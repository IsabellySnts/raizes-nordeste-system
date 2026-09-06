namespace RaizesDoNordeste.Domain.Entities;

public class Cardapio : BaseEntity
{
    public long IdProduto { get; private set; }
    public long IdUnidade { get; private set; }
    public bool Disponivel { get; private set; }
    public decimal? PrecoLocal { get; private set; }
    public string? VariacaoRegional { get; private set; }
    public Produto? Produto { get; private set; }
    public Unidade? Unidade { get; private set; }

    protected Cardapio() { }

    public Cardapio(long idProduto, long idUnidade, bool disponivel, decimal? precoLocal, string? variacaoRegional)
    {
        IdProduto = idProduto;
        IdUnidade = idUnidade;
        Disponivel = disponivel;
        PrecoLocal = precoLocal;
        VariacaoRegional = variacaoRegional;
    }

    public void AtualizarDisponibilidade(bool disponivel)
    {
        Disponivel = disponivel;
    }

    public void AtualizarPrecoLocal(decimal? preco)
    {
        PrecoLocal = preco;
    }
}
