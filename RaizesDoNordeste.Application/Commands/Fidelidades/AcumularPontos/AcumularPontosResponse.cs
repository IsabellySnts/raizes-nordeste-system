namespace RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;

public class AcumularPontosResponse
{
    public int PontosAcumulados { get; set; }
    public int SaldoAnterior { get; set; }
    public int SaldoAtual { get; set; }
    public string Nivel { get; set; } = string.Empty;
}