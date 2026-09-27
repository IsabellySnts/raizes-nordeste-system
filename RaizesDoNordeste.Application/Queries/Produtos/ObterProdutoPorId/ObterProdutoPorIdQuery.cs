using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Produtos.ObterProdutoPorId;

public sealed record ObterProdutoPorIdQuery : IRequest<ResultViewModel<ProdutoQueryResponse>>
{
    public long Id { get; init; }
}
