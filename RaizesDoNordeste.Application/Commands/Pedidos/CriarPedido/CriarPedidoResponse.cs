namespace RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;

public class CriarPedidoResponse
{
    public long Id { get; set; }
    public long? IdCliente { get; set; }
    public long IdUnidade { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public string CanalOrigem { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public List<ItemPedidoResponse> Itens { get; set; } = new();
}

public class ItemPedidoResponse
{
    public long Id { get; set; }
    public long IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Observacao { get; set; }
}