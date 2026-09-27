namespace RaizesDoNordeste.Application.Commands.Estoques.CriarEstoque;

public class CriarEstoqueResponse
{
    public long Id { get; set; }
    public long IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public long IdUnidade { get; set; }
    public int Quantidade { get; set; }
    public int QuantidadeMinima { get; set; }
    public bool Disponivel { get; set; }
    public DateTime DataInsercao { get; set; }
}