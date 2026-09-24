using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Funcionarios.ObterTodosFuncionarios;

public class ObterTodosFuncionariosHandler(IFuncionarioRepository _repository) : IRequestHandler<ObterTodosFuncionariosQuery, ResultViewModel<IEnumerable<FuncionarioQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<FuncionarioQueryResponse>>> Handle(
        ObterTodosFuncionariosQuery query, CancellationToken cancellationToken)
    {
        var funcionarios = query.IdUnidade.HasValue
            ? await _repository.ObterPorUnidadeAsync(query.IdUnidade.Value)
            : await _repository.ObterTodosAsync();

        var response = funcionarios.Select(f => new FuncionarioQueryResponse
        {
            Id = f.Id,
            Nome = f.Nome,
            CpfMascarado = MascararCpf(f.Cpf),
            Email = f.Email,
            Telefone = f.Telefone,
            IdUnidade = f.IdUnidade,
            NomeUnidade = f.Unidade?.Nome,
            Cargo = f.Cargo.ToString(),
            Ativo = f.Ativo
        });

        return ResultViewModel<IEnumerable<FuncionarioQueryResponse>>.Success(response);
    }

    private static string MascararCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || cpf.Length < 11)
            return "***";
        return $"***.***.**{cpf[^3..]}";
    }
}
