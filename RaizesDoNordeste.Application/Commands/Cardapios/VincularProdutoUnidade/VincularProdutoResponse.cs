namespace RaizesDoNordeste.Application.Commands.Cardapios.VincularProdutoUnidade;

public class VincularProdutoResponse
{
    public long Id { get; set; }
    public long IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public long IdUnidade { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public bool Disponivel { get; set; }
    public decimal PrecoExibido { get; set; }
    public string? VariacaoRegional { get; set; }
}