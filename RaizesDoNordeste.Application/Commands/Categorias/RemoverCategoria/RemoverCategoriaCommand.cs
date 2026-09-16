using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Categorias.RemoverCategoria;

public sealed record RemoverCategoriaCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}