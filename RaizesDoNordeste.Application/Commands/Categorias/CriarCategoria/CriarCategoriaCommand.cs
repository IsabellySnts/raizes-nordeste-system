using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;

public sealed record CriarCategoriaCommand : IRequest<ResultViewModel<CriarCategoriaResponse>>
{
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
}
