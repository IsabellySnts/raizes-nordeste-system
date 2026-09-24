using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Estoques.AjustarEstoque;

public sealed record AjustarEstoqueCommand : IRequest<ResultViewModel<AjustarEstoqueResponse>>
{
    public long IdUnidade { get; init; }
    public long IdProduto { get; init; }
    public int NovaQuantidade { get; init; }
    public int? NovaQuantidadeMinima { get; init; }
}
