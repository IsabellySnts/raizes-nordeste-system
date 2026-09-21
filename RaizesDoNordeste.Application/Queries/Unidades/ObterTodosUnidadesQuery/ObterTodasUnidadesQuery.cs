using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Unidades.ObterTodosUnidadesQuery;

public sealed record ObterTodasUnidadesQuery : IRequest<ResultViewModel<IEnumerable<UnidadeQueryResponse>>>
{
}
