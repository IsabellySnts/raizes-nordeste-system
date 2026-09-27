using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Clientes.AnonimizarCliente;

public sealed record AnonimizarClienteCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}
