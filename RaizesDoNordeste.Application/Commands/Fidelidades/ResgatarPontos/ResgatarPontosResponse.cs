namespace RaizesDoNordeste.Application.Commands.Fidelidades.ResgatarPontos;

public class ResgatarPontosResponse
{
    public int PontosResgatados { get; set; }
    public decimal ValorDesconto { get; set; }
    public int SaldoAnterior { get; set; }
    public int SaldoAtual { get; set; }
    public string Nivel { get; set; } = string.Empty;
}