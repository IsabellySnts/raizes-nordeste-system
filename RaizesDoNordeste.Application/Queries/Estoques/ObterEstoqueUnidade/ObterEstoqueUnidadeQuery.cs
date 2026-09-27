using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Estoques.ObterEstoqueUnidade;

public sealed record ObterEstoqueUnidadeQuery : IRequest<ResultViewModel<IEnumerable<EstoqueQueryResponse>>>
{
    public long IdUnidade { get; init; }
}
