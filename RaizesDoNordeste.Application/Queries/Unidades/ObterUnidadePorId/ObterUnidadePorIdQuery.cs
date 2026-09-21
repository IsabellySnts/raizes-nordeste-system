using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Unidades.ObterUnidadePorId;

public sealed record ObterUnidadePorIdQuery : IRequest<ResultViewModel<UnidadeQueryResponse>>
{
    public long Id { get; init; }
}