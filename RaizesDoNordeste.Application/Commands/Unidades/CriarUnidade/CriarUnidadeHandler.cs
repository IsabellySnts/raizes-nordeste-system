using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Unidades.CriarUnidade;

public class CriarUnidadeHandler(IUnidadeRepository _repository) : IRequestHandler<CriarUnidadeCommand, ResultViewModel<CriarUnidadeResponse>>
{
    public async Task<ResultViewModel<CriarUnidadeResponse>> Handle(CriarUnidadeCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarUnidadeValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarUnidadeResponse>.Error(erros);
        }

        var nomeExiste = await _repository.NomeExisteAsync(command.Nome);
        if (nomeExiste)
            return ResultViewModel<CriarUnidadeResponse>.Error("Já existe uma unidade com este nome.");

        var unidade = new Unidade(
            command.Nome,
            command.Cidade,
            command.Estado,
            command.Pais,
            command.Logradouro,
            command.Complemento,
            command.DiasFuncionamento,
            command.HorarioFuncionamento,
            command.TipoCozinha
        );

        var unidadeCriada = await _repository.CriarAsync(unidade);

        var response = new CriarUnidadeResponse
        {
            Id = unidadeCriada.Id,
            Nome = unidadeCriada.Nome,
            Cidade = unidadeCriada.Cidade,
            Estado = unidadeCriada.Estado,
            Pais = unidadeCriada.Pais,
            Logradouro = unidadeCriada.Logradouro,
            Complemento = unidadeCriada.Complemento,
            DiasFuncionamento = unidadeCriada.DiasFuncionamento,
            HorarioFuncionamento = unidadeCriada.HorarioFuncionamento,
            TipoCozinha = unidadeCriada.TipoCozinha.ToString()
        };

        return ResultViewModel<CriarUnidadeResponse>.Success(response);
    }
}
