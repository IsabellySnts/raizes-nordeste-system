namespace RaizesDoNordeste.Application.Commands.Categorias.AlterarCategoria;

public class AlterarCategoriaResponse
{
    public long IdCategoria { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
