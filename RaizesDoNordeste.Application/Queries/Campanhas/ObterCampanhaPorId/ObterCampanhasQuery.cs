using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Campanhas.ObterCampanhaPorId;

public sealed record ObterCampanhasQuery : IRequest<ResultViewModel<IEnumerable<CampanhaQueryResponse>>>
{
    public bool ApenasAtivas { get; init; } = false;
}
