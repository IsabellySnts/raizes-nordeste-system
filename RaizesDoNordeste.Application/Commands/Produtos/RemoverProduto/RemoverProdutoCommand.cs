using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Produtos.RemoverProduto;

public sealed record RemoverProdutoCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}
