namespace RaizesDoNordeste.Application.Queries.Pagamentos.ObterPagamentoPorId;

public class PagamentoResponse
{
    public long Id { get; set; }
    public long IdPedido { get; set; }
    public decimal Valor { get; set; }
    public string TipoPagamento { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DataSolicitacao { get; set; }
    public DateTime? DataEfetivacao { get; set; }
    public string? CodigoTransacao { get; set; }
    public int Tentativas { get; set; }
}