namespace RaizesDoNordeste.Domain.Entities;

public class Produto : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public long IdCategoria { get; private set; }
    public string? Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public bool FlagSazonal { get; private set; }
    public DateTime? DataInicioDisponibilidade { get; private set; }
    public DateTime? DataFimDisponibilidade { get; private set; }
    public string? Foto { get; private set; }
    public Categoria? Categoria { get; private set; }
    public ICollection<Cardapio> Cardapios { get; private set; } = new List<Cardapio>();
    public ICollection<Estoque> Estoques { get; private set; } = new List<Estoque>();

    protected Produto() { }

    public Produto(string nome, long idCategoria, string? descricao, decimal preco,
                   bool flagSazonal, DateTime? dataInicio, DateTime? dataFim, string? foto)
    {
        Nome = nome;
        IdCategoria = idCategoria;
        Descricao = descricao;
        Preco = preco;
        FlagSazonal = flagSazonal;
        DataInicioDisponibilidade = dataInicio;
        DataFimDisponibilidade = dataFim;
        Foto = foto;
    }

    public bool EstaDisponivel(DateTime dataAtual)
    {
        if (!FlagSazonal) return true;
        if (DataInicioDisponibilidade == null || DataFimDisponibilidade == null) return true;
        return dataAtual >= DataInicioDisponibilidade && dataAtual <= DataFimDisponibilidade;
    }
}
