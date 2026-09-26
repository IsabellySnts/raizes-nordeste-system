using RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;

namespace RaizesDoNordeste.Application.Queries.Pedidos;

public class PedidoQueryResponse
{
    public long Id { get; set; }
    public long? IdCliente { get; set; }
    public string? NomeCliente { get; set; }
    public long IdUnidade { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public long? IdFuncionario { get; set; }
    public string? NomeFuncionario { get; set; }
    public string CanalOrigem { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public string? StatusPagamento { get; set; }
    public List<ItemPedidoResponse> Itens { get; set; } = new();
}
