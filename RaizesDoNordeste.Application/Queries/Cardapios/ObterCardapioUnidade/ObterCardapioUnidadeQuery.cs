using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Cardapios.ObterCardapioUnidade;

public sealed record ObterCardapioUnidadeQuery : IRequest<ResultViewModel<IEnumerable<CardapioQueryResponse>>>
{
    public long IdUnidade { get; init; }
    public bool ApenasDisponiveis { get; init; } = true;
}
