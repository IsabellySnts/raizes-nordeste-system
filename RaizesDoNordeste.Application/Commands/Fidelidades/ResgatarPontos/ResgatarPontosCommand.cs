using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.ResgatarPontos;

public sealed record ResgatarPontosCommand : IRequest<ResultViewModel<ResgatarPontosResponse>>
{
    public long IdCliente { get; init; }
    public long? IdPedido { get; init; }
    public int Pontos { get; init; }
}
