using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Pedidos.AtualizarStatusPedido;

public sealed record AtualizarStatusPedidoCommand : IRequest<ResultViewModel<AtualizarStatusPedidoResponse>>
{
    public long IdPedido { get; init; }
    public StatusPedido NovoStatus { get; init; }
}
