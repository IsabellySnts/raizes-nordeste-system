using RaizesDoNordeste.Domain.Aggregates;

namespace RaizesDoNordeste.Domain.Entities;

public class ItemPedido : BaseEntity
{
    public long IdPedido { get; private set; }
    public long IdProduto { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public string? Observacao { get; private set; }
    public Pedido? Pedido { get; private set; }
    public Produto? Produto { get; private set; }

    protected ItemPedido() { }

    public ItemPedido(long idPedido, long idProduto, int quantidade, decimal precoUnitario, string? observacao)
    {
        IdPedido = idPedido;
        IdProduto = idProduto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        Observacao = observacao;
    }
}
