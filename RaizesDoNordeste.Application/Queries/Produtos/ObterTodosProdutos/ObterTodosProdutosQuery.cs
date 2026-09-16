using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Produtos.ObterTodosProdutos;

public sealed record ObterTodosProdutosQuery : IRequest<ResultViewModel<IEnumerable<ProdutoQueryResponse>>>
{
}