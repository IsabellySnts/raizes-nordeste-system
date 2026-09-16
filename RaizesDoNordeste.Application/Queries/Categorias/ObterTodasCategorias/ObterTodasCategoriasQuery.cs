using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Categorias.ObterTodasCategorias;

public sealed record ObterTodasCategoriasQuery : IRequest<ResultViewModel<IEnumerable<CategoriaQueryResponse>>>
{
}
