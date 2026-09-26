namespace RaizesDoNordeste.Application.Commands.Auditoria.RegistrarAuditoria;

public class RegistrarAuditoriaResponse
{
    public long Id { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    public string TipoEntidadeAfetada { get; set; } = string.Empty;
    public long IdEntidadeAfetada { get; set; }
    public string? Detalhes { get; set; }
    public DateTime DataHora { get; set; }
}