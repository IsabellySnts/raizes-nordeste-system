namespace RaizesDoNordeste.Application.Commands.Campanhas.CriarCampanha;

public class CriarCampanhaResponse
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Criterios { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Beneficio { get; set; }
}