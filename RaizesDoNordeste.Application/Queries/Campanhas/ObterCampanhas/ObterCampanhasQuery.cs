using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Campanhas.ObterCampanhas;

public sealed record ObterCampanhasQuery : IRequest<ResultViewModel<IEnumerable<CampanhaQueryResponse>>>
{
    public bool ApenasAtivas { get; init; } = false;
}
