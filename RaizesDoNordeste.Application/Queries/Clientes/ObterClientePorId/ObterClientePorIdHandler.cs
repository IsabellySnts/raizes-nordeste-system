using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Clientes.ObterClientePorId;

public class ObterClientePorIdHandler(IClienteRepository _repository) : IRequestHandler<ObterClientePorIdQuery, ResultViewModel<ClienteQueryResponse>>
{
    public async Task<ResultViewModel<ClienteQueryResponse>> Handle(
        ObterClientePorIdQuery query, CancellationToken cancellationToken)
    {
        var cliente = await _repository.ObterPorIdComUsuarioAsync(query.Id);

        if (cliente == null)
            return ResultViewModel<ClienteQueryResponse>.Error("Cliente não encontrado.");

        var response = new ClienteQueryResponse
        {
            Id = cliente.Id,
            NomeCompleto = cliente.NomeCompleto,
            CpfMascarado = MascararCpf(cliente.Cpf),
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            DataNascimento = cliente.DataNascimento,
            DataCadastro = cliente.DataCadastro,
            SaldoPontos = cliente.Fidelidade?.SaldoPontos,
            NivelFidelidade = cliente.Fidelidade?.Nivel.ToString()
        };

        return ResultViewModel<ClienteQueryResponse>.Success(response);
    }

    private static string MascararCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || cpf == "ANONIMIZADO" || cpf.Length < 11)
            return "***";

        return $"***.***.**{cpf[^3..]}";
    }
}
