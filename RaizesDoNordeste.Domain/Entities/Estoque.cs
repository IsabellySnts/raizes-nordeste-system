namespace RaizesDoNordeste.Domain.Entities;

public class Estoque : BaseEntity
{
    public long IdProduto { get; private set; }
    public long IdUnidade { get; private set; }
    public int Quantidade { get; private set; }
    public int QuantidadeMinima { get; private set; }
    public DateTime DataInsercao { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public Produto? Produto { get; private set; }
    public Unidade? Unidade { get; private set; }

    protected Estoque() { }

    public Estoque(long idProduto, long idUnidade, int quantidade, int quantidadeMinima)
    {
        IdProduto = idProduto;
        IdUnidade = idUnidade;
        Quantidade = quantidade;
        QuantidadeMinima = quantidadeMinima;
        DataInsercao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool Reduzir(int qtd)
    {
        if (Quantidade < qtd) return false;
        Quantidade -= qtd;
        DataAtualizacao = DateTime.UtcNow;
        return true;
    }

    public void Repor(int qtd)
    {
        Quantidade += qtd;
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool EstaDisponivel()
    {
        return Quantidade > QuantidadeMinima;
    }
}