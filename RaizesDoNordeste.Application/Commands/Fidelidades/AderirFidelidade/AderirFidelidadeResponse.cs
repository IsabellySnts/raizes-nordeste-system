namespace RaizesDoNordeste.Application.Commands.Fidelidades.AderirFidelidade;

public class AderirFidelidadeResponse
{
    public long Id { get; set; }
    public long IdCliente { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int SaldoPontos { get; set; }
    public string Nivel { get; set; } = string.Empty;
}