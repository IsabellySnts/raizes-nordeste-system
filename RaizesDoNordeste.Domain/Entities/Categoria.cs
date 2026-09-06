namespace RaizesDoNordeste.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public ICollection<Produto> Produtos { get; private set; } = new List<Produto>();

    protected Categoria() { }

    public Categoria(string nome, string? descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
