using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidoPorId;

public sealed record ObterPedidoPorIdQuery : IRequest<ResultViewModel<PedidoQueryResponse>>
{
    public long Id { get; init; }
}
