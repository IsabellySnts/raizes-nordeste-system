namespace RaizesDoNordeste.Application.Queries.Auditorias;

public class AuditoriaQueryResponse
{
    public long Id { get; set; }
    public long IdFuncionario { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public string? CargoFuncionario { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string TipoEntidadeAfetada { get; set; } = string.Empty;
    public long IdEntidadeAfetada { get; set; }
    public string? Detalhes { get; set; }
    public DateTime DataHora { get; set; }
}
