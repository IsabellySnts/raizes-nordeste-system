using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Clientes.AtualizarCliente;

public sealed record AtualizarClienteCommand : IRequest<ResultViewModel<AtualizarClienteResponse>>
{
    public long Id { get; init; }
    public string NomeCompleto { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
}
