using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Funcionarios.ObterFuncionarioPorId;

public class ObterFuncionarioPorIdHandler(IFuncionarioRepository _repository) : IRequestHandler<ObterFuncionarioPorIdQuery, ResultViewModel<FuncionarioQueryResponse>>
{
    public async Task<ResultViewModel<FuncionarioQueryResponse>> Handle(
        ObterFuncionarioPorIdQuery query, CancellationToken cancellationToken)
    {
        var funcionario = await _repository.ObterPorIdComDetalhesAsync(query.Id);

        if (funcionario == null)
            return ResultViewModel<FuncionarioQueryResponse>.Error("Funcionário não encontrado.");

        var response = new FuncionarioQueryResponse
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            CpfMascarado = MascararCpf(funcionario.Cpf),
            Email = funcionario.Email,
            Telefone = funcionario.Telefone,
            IdUnidade = funcionario.IdUnidade,
            NomeUnidade = funcionario.Unidade?.Nome,
            Cargo = funcionario.Cargo.ToString(),
            Ativo = funcionario.Ativo
        };

        return ResultViewModel<FuncionarioQueryResponse>.Success(response);
    }

    private static string MascararCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || cpf.Length < 11)
            return "***";
        return $"***.***.**{cpf[^3..]}";
    }
}