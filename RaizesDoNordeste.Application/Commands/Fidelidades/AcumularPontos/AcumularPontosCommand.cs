using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;

public sealed record AcumularPontosCommand : IRequest<ResultViewModel<AcumularPontosResponse>>
{
    public long IdCliente { get; init; }
    public long? IdPedido { get; init; }
    public decimal ValorPedido { get; init; }
}
