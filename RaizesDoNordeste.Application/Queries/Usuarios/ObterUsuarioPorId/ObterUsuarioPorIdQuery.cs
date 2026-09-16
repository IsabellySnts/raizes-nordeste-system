using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Usuarios.ObterUsuarioPorId;

public sealed record ObterUsuarioPorIdQuery : IRequest<ResultViewModel<UsuarioQueryResponse>>
{
    public long Id { get; init; }
}
