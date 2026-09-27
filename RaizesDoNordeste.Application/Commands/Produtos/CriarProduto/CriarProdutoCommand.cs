using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Produtos.CriarProduto;

public sealed record CriarProdutoCommand : IRequest<ResultViewModel<CriarProdutoResponse>>
{
    public string Nome { get; init; } = string.Empty;
    public long IdCategoria { get; init; }
    public string? Descricao { get; init; }
    public decimal Preco { get; init; }
    public bool FlagSazonal { get; init; }
    public DateTime? DataInicioDisponibilidade { get; init; }
    public DateTime? DataFimDisponibilidade { get; init; }
    public string? Foto { get; init; }
}
