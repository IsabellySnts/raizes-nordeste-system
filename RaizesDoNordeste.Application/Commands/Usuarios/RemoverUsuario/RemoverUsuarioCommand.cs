using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Usuarios.RemoverUsuario;

public sealed record RemoverUsuarioCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}
