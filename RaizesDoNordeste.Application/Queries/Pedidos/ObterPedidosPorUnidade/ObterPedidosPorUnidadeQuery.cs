using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidosPorUnidade;

public sealed record ObterPedidosPorUnidadeQuery : IRequest<ResultViewModel<IEnumerable<PedidoResumoResponse>>>
{
    public long IdUnidade { get; init; }
}

public class PedidoResumoResponse
{
    public long Id { get; set; }
    public string? NomeCliente { get; set; }
    public string CanalOrigem { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public int TotalItens { get; set; }
    public DateTime DataCriacao { get; set; }
}
