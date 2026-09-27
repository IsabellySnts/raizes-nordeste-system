namespace RaizesDoNordeste.Application.Commands.Estoques.AjustarEstoque;

public class AjustarEstoqueResponse
{
    public long Id { get; set; }
    public long IdProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public long IdUnidade { get; set; }
    public int QuantidadeAnterior { get; set; }
    public int QuantidadeAtual { get; set; }
    public int QuantidadeMinima { get; set; }
    public bool Disponivel { get; set; }
    public DateTime DataAtualizacao { get; set; }
}