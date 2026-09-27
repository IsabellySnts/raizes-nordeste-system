using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Campanhas.AtualizarCampanha;

public class AtualizarCampanhaHandler(ICampanhaRepository _repository) : IRequestHandler<AtualizarCampanhaCommand, ResultViewModel<AtualizarCampanhaResponse>>
{
    public async Task<ResultViewModel<AtualizarCampanhaResponse>> Handle(
        AtualizarCampanhaCommand command, CancellationToken cancellationToken)
    {
        var validator = new AtualizarCampanhaCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AtualizarCampanhaResponse>.Error(erros);
        }

        var campanha = await _repository.ObterPorIdAsync(command.Id);
        if (campanha == null)
            return ResultViewModel<AtualizarCampanhaResponse>.Error("Campanha não encontrada.");

        var nomeExiste = await _repository.NomeExisteAsync(command.Nome, command.Id);
        if (nomeExiste)
            return ResultViewModel<AtualizarCampanhaResponse>.Error("Já existe outra campanha com este nome.");

        campanha.Atualizar(
            command.Nome,
            command.Descricao,
            command.Criterios,
            command.DataInicio,
            command.DataFim,
            command.Beneficio
        );

        await _repository.AtualizarAsync(campanha);

        var response = new AtualizarCampanhaResponse
        {
            Id = campanha.Id,
            Nome = campanha.Nome,
            Descricao = campanha.Descricao,
            Criterios = campanha.Criterios,
            DataInicio = campanha.DataInicio,
            DataFim = campanha.DataFim,
            Status = campanha.Status.ToString(),
            Beneficio = campanha.Beneficio
        };

        return ResultViewModel<AtualizarCampanhaResponse>.Success(response);
    }
}
