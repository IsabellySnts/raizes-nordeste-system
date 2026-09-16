using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Usuarios.ObterTodosUsuarios;

public sealed record ObterTodosUsuariosQuery : IRequest<ResultViewModel<IEnumerable<UsuarioQueryResponse>>>
{
}
