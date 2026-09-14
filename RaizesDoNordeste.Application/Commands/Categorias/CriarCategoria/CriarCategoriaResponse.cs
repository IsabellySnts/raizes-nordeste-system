namespace RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;

public class CriarCategoriaResponse
{
    public long IdCategoria { get; set; }
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
}
