using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class Pagamento : BaseEntity
{
    public long IdPedido { get; private set; }
    public decimal Valor { get; private set; }
    public TipoPagamento TipoPagamento { get; private set; }
    public StatusPagamento Status { get; private set; }
    public DateTime DataSolicitacao { get; private set; }
    public DateTime? DataEfetivacao { get; private set; }
    public string? CodigoTransacao { get; private set; }
    public int Tentativas { get; private set; }
    public Pedido? Pedido { get; private set; }

    protected Pagamento() { }

    public Pagamento(long idPedido, decimal valor, TipoPagamento tipoPagamento)
    {
        IdPedido = idPedido;
        Valor = valor;
        TipoPagamento = tipoPagamento;
        Status = StatusPagamento.Pendente;
        DataSolicitacao = DateTime.UtcNow;
        Tentativas = 0;
    }

    public void RegistrarRetorno(StatusPagamento status, string? codigoTransacao)
    {
        Status = status;
        CodigoTransacao = codigoTransacao;
        DataEfetivacao = DateTime.UtcNow;
    }

    public void IncrementarTentativa()
    {
        Tentativas++;
    }

    public bool PodeRetentar()
    {
        return Tentativas < 3 && Status != StatusPagamento.Aprovado;
    }
}