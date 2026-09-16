using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Usuarios.AtualizarUsuario;

public sealed record AtualizarUsuarioCommand : IRequest<ResultViewModel<AtualizarUsuarioResponse>>
{
    public long Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? NovaSenha { get; init; }
    public string? ConfirmarNovaSenha { get; init; }
}
