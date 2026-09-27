namespace RaizesDoNordeste.Application.Queries.Cardapios;

public class CardapioQueryResponse
{
    public long Id { get; set; }
    public long IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public string? DescricaoProduto { get; set; }
    public string? CategoriaProduto { get; set; }
    public decimal PrecoExibido { get; set; }
    public bool Disponivel { get; set; }
    public string? VariacaoRegional { get; set; }
    public string? Foto { get; set; }
    public bool Sazonal { get; set; }
}
