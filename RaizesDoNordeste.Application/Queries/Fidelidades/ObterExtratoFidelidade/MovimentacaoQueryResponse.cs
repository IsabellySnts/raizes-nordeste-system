namespace RaizesDoNordeste.Application.Queries.Fidelidades.ObterExtratoFidelidade;

public class MovimentacaoQueryResponse
{
    public long Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Pontos { get; set; }
    public long? IdPedido { get; set; }
    public DateTime Data { get; set; }
}

public class ExtratoFidelidadeResponse
{
    public long IdCliente { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int SaldoAtual { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public IEnumerable<MovimentacaoQueryResponse> Movimentacoes { get; set; } = [];
}