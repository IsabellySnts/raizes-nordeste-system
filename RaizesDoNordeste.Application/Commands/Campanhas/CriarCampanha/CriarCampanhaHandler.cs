using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Campanhas.CriarCampanha;

public class CriarCampanhaHandler(ICampanhaRepository _repository) : IRequestHandler<CriarCampanhaCommand, ResultViewModel<CriarCampanhaResponse>>
{
    public async Task<ResultViewModel<CriarCampanhaResponse>> Handle(CriarCampanhaCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarCampanhaCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarCampanhaResponse>.Error(erros);
        }

        var nomeExiste = await _repository.NomeExisteAsync(command.Nome);
        if (nomeExiste)
            return ResultViewModel<CriarCampanhaResponse>.Error("Já existe uma campanha com este nome.");

        var campanha = new Campanha(
            command.Nome,
            command.Descricao,
            command.Criterios,
            command.DataInicio,
            command.DataFim,
            command.Beneficio
        );

        var campanhaCriada = await _repository.CriarAsync(campanha);

        var response = new CriarCampanhaResponse
        {
            Id = campanhaCriada.Id,
            Nome = campanhaCriada.Nome,
            Descricao = campanhaCriada.Descricao,
            Criterios = campanhaCriada.Criterios,
            DataInicio = campanhaCriada.DataInicio,
            DataFim = campanhaCriada.DataFim,
            Status = campanhaCriada.Status.ToString(),
            Beneficio = campanhaCriada.Beneficio
        };

        return ResultViewModel<CriarCampanhaResponse>.Success(response);
    }
}
