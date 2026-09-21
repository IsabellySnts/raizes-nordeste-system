using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Clientes.ObterClientePorId;

public sealed record ObterClientePorIdQuery : IRequest<ResultViewModel<ClienteQueryResponse>>
{
    public long Id { get; init; }
}
