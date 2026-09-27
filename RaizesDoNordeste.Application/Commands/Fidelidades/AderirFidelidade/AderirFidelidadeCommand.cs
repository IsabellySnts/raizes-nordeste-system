using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.AderirFidelidade;

public sealed record AderirFidelidadeCommand : IRequest<ResultViewModel<AderirFidelidadeResponse>>
{
    public long IdCliente { get; init; }
}
