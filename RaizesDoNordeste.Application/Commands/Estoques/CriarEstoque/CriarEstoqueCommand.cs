using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Estoques.CriarEstoque;

public sealed record CriarEstoqueCommand : IRequest<ResultViewModel<CriarEstoqueResponse>>
{
    public long IdUnidade { get; init; }
    public long IdProduto { get; init; }
    public int Quantidade { get; init; }
    public int QuantidadeMinima { get; init; }
}
