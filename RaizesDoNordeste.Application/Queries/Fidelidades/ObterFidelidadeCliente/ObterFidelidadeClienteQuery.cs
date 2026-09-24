using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Fidelidades.ObterFidelidadeCliente;

public sealed record ObterFidelidadeClienteQuery : IRequest<ResultViewModel<FidelidadeQueryResponse>>
{
    public long IdCliente { get; init; }
}
