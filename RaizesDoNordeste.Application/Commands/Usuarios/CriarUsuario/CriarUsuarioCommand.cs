using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Usuarios.CriarUsuario;

public sealed record CriarUsuarioCommand : IRequest<ResultViewModel<CriarUsuarioResponse>>
{
    public string Email { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
    public string ConfirmarSenha { get; init; } = string.Empty;
}
