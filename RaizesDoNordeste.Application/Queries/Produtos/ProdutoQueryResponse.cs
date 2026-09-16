namespace RaizesDoNordeste.Application.Queries.Produtos;

public class ProdutoQueryResponse
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public long IdCategoria { get; set; }
    public string? NomeCategoria { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public bool FlagSazonal { get; set; }
    public DateTime? DataInicioDisponibilidade { get; set; }
    public DateTime? DataFimDisponibilidade { get; set; }
    public string? Foto { get; set; }
}