namespace RaizesDoNordeste.Application.Queries.Fidelidades;

public class FidelidadeQueryResponse
{
    public long Id { get; set; }
    public long IdCliente { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int SaldoPontos { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public decimal ValorEmDesconto { get; set; }
}
