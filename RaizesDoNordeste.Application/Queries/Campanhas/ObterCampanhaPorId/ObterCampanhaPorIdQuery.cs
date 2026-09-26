using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Campanhas.ObterCampanhaPorId;

public sealed record ObterCampanhaPorIdQuery : IRequest<ResultViewModel<CampanhaQueryResponse>>
{
    public long Id { get; init; }
}
