namespace RaizesDoNordeste.Application.Commands.Unidades.AtualizarUnidade;

public class AtualizarUnidadeResponse
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string DiasFuncionamento { get; set; } = string.Empty;
    public string HorarioFuncionamento { get; set; } = string.Empty;
    public string TipoCozinha { get; set; } = string.Empty;
}