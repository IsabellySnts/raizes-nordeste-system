using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Cardapios.VincularProdutoUnidade;

public sealed record VincularProdutoCommand : IRequest<ResultViewModel<VincularProdutoResponse>>
{
    public long IdUnidade { get; init; }
    public long IdProduto { get; init; }
    public bool Disponivel { get; init; } = true;
    public decimal? PrecoLocal { get; init; }
    public string? VariacaoRegional { get; init; }
}