namespace RaizesDoNordeste.Domain.Entities;

public class Auditoria : BaseEntity
{
    public long IdFuncionario { get; private set; }
    public string Acao { get; private set; }
    public string TipoEntidadeAfetada { get; private set; } = string.Empty;
    public long IdEntidadeAfetada { get; private set; }
    public string? Detalhes { get; private set; }
    public DateTime DataHora { get; private set; }
    public Funcionario? Funcionario { get; private set; }

    protected Auditoria() { }

    public Auditoria(long idFuncionario, string acao, string tipoEntidadeAfetada, long idEntidadeAfetada, string? detalhes)
    {
        IdFuncionario = idFuncionario;
        Acao = acao;
        TipoEntidadeAfetada = tipoEntidadeAfetada;
        IdEntidadeAfetada = idEntidadeAfetada;
        Detalhes = detalhes;
        DataHora = DateTime.UtcNow;
    }
}
