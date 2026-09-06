using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Aggregates;

public class Pedido : BaseEntity
{
    public long? IdCliente { get; private set; }
    public long IdUnidade { get; private set; }
    public long? IdFuncionario { get; private set; }
    public CanalOrigem CanalOrigem { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public Cliente? Cliente { get; private set; }
    public Unidade? Unidade { get; private set; }
    public Funcionario? Funcionario { get; private set; }
    public Pagamento? Pagamento { get; private set; }
    public ICollection<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();

    protected Pedido() { }

    public Pedido(long? idCliente, long idUnidade, long? idFuncionario, CanalOrigem canalOrigem)
    {
        IdCliente = idCliente;
        IdUnidade = idUnidade;
        IdFuncionario = idFuncionario;
        CanalOrigem = canalOrigem;
        Status = StatusPedido.Criado;
        ValorTotal = 0;
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AdicionarItem(ItemPedido item)
    {
        Itens.Add(item);
        CalcularTotal();
    }

    public void RemoverItem(long itemId)
    {
        var item = Itens.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Itens.Remove(item);
            CalcularTotal();
        }
    }

    public void CalcularTotal()
    {
        ValorTotal = Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool AtualizarStatus(StatusPedido novoStatus)
    {
        var transicaoValida = (Status, novoStatus) switch
        {
            (StatusPedido.Criado, StatusPedido.AguardandoPagamento) => true,
            (StatusPedido.AguardandoPagamento, StatusPedido.Pago) => true,
            (StatusPedido.AguardandoPagamento, StatusPedido.Cancelado) => true,
            (StatusPedido.AguardandoPagamento, StatusPedido.Recusado) => true,
            (StatusPedido.AguardandoPagamento, StatusPedido.ErroPagamento) => true,
            (StatusPedido.Pago, StatusPedido.EmPreparo) => true,
            (StatusPedido.Pago, StatusPedido.Cancelado) => true,
            (StatusPedido.EmPreparo, StatusPedido.Pronto) => true,
            (StatusPedido.EmPreparo, StatusPedido.Cancelado) => true,
            (StatusPedido.Pronto, StatusPedido.Entregue) => true,
            (StatusPedido.Pronto, StatusPedido.Cancelado) => true,
            _ => false
        };

        if (!transicaoValida) return false;

        Status = novoStatus;
        DataAtualizacao = DateTime.UtcNow;
        return true;
    }

    public bool PodeCancelarPeloCliente()
    {
        return Status == StatusPedido.AguardandoPagamento || Status == StatusPedido.Pago;
    }
}
