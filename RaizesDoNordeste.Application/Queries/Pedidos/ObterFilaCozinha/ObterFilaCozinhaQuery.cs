using MediatR;
using RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Pedidos.ObterFilaCozinha;

public sealed record ObterFilaCozinhaQuery : IRequest<ResultViewModel<IEnumerable<FilaCozinhaResponse>>>
{
    public long IdUnidade { get; init; }
}

public class FilaCozinhaResponse
{
    public long IdPedido { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public List<ItemPedidoResponse> Itens { get; set; } = new();
}
