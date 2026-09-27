namespace RaizesDoNordeste.Application.Queries.Estoques;

public class EstoqueQueryResponse
{
    public long Id { get; set; }
    public long IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public string? CategoriaProduto { get; set; }
    public int Quantidade { get; set; }
    public int QuantidadeMinima { get; set; }
    public bool Disponivel { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataAtualizacao { get; set; }
}
