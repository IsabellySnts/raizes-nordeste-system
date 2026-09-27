using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;
using RaizesDoNordeste.Domain.Interfaces.Services;

namespace RaizesDoNordeste.Application.Commands.Usuarios.Login;

public class LoginHandler(IUsuarioRepository _usuarioRepository, IAuthService _serviceAuth) : IRequestHandler<LoginCommand, ResultViewModel<LoginResponse>>
{
    public async Task<ResultViewModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

        if (usuario is null)
            return ResultViewModel<LoginResponse>.Error("Usuário não encontrado.");

        if (!_serviceAuth.VerifyHash(request.Senha, usuario.Senha))
            return ResultViewModel<LoginResponse>.Error("Senha inválida!");

        var token = _serviceAuth.GenerateToken(usuario);

        return ResultViewModel<LoginResponse>.Success(new LoginResponse(token), "Usuário autenticado!");

    }
}
