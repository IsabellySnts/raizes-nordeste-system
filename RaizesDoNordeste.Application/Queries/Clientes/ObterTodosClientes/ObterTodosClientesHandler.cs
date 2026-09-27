using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Clientes.ObterTodosClientes;

public class ObterTodosClientesHandler(IClienteRepository _repository): IRequestHandler<ObterTodosClientesQuery, ResultViewModel<IEnumerable<ClienteQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<ClienteQueryResponse>>> Handle(ObterTodosClientesQuery query, CancellationToken cancellationToken)
    {
        var clientes = await _repository.ObterTodosAsync();

        var response = clientes.Select(c => new ClienteQueryResponse
        {
            Id = c.Id,
            NomeCompleto = c.NomeCompleto,
            CpfMascarado = MascararCpf(c.Cpf),
            Email = c.Email,
            Telefone = c.Telefone,
            DataNascimento = c.DataNascimento,
            DataCadastro = c.DataCadastro,
            SaldoPontos = c.Fidelidade?.SaldoPontos,
            NivelFidelidade = c.Fidelidade?.Nivel.ToString()
        });

        return ResultViewModel<IEnumerable<ClienteQueryResponse>>.Success(response);
    }

    private static string MascararCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || cpf == "ANONIMIZADO" || cpf.Length < 11)
            return "***";

        return $"***.***.**{cpf[^3..]}";
    }
}
