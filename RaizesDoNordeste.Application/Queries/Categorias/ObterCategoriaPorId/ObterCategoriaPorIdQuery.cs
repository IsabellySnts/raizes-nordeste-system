using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Categorias.ObterCategoriaPorId;

public sealed record ObterCategoriaPorIdQuery : IRequest<ResultViewModel<CategoriaQueryResponse>>
{
    public long Id { get; init; }
}
