using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Unidades.AtualizarUnidade;

public class AtualizarUnidadeHandler(IUnidadeRepository _repository)
    : IRequestHandler<AtualizarUnidadeCommand, ResultViewModel<AtualizarUnidadeResponse>>
{
    public async Task<ResultViewModel<AtualizarUnidadeResponse>> Handle(
        AtualizarUnidadeCommand command, CancellationToken cancellationToken)
    {
        var validator = new AtualizarUnidadeValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AtualizarUnidadeResponse>.Error(erros);
        }

        var unidade = await _repository.ObterPorIdAsync(command.Id);
        if (unidade == null)
            return ResultViewModel<AtualizarUnidadeResponse>.Error("Unidade não encontrada.");

        var nomeExiste = await _repository.NomeExisteAsync(command.Nome, command.Id);
        if (nomeExiste)
            return ResultViewModel<AtualizarUnidadeResponse>.Error("Já existe outra unidade com este nome.");

        unidade.Atualizar(
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

        await _repository.AtualizarAsync(unidade);

        var response = new AtualizarUnidadeResponse
        {
            Id = unidade.Id,
            Nome = unidade.Nome,
            Cidade = unidade.Cidade,
            Estado = unidade.Estado,
            Pais = unidade.Pais,
            Logradouro = unidade.Logradouro,
            Complemento = unidade.Complemento,
            DiasFuncionamento = unidade.DiasFuncionamento,
            HorarioFuncionamento = unidade.HorarioFuncionamento,
            TipoCozinha = unidade.TipoCozinha.ToString()
        };

        return ResultViewModel<AtualizarUnidadeResponse>.Success(response);
    }
}
