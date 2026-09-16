using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Usuarios.AtualizarUsuario;

public class AtualizarUsuarioHandler(IUsuarioRepository _repository) : IRequestHandler<AtualizarUsuarioCommand, ResultViewModel<AtualizarUsuarioResponse>>
{
    public async Task<ResultViewModel<AtualizarUsuarioResponse>> Handle(
        AtualizarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var validator = new AtualizarUsuarioCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AtualizarUsuarioResponse>.Error(erros);
        }

        var usuario = await _repository.ObterPorIdAsync(command.Id);
        if (usuario == null)
            return ResultViewModel<AtualizarUsuarioResponse>.Error("Usuário não encontrado.");

        var emailExiste = await _repository.EmailExisteAsync(command.Email, command.Id);
        if (emailExiste)
            return ResultViewModel<AtualizarUsuarioResponse>.Error("Este email já está em uso por outro usuário.");

        usuario.AtualizarEmail(command.Email);

        if (!string.IsNullOrEmpty(command.NovaSenha))
        {
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(command.NovaSenha);
            usuario.AtualizarSenha(senhaHash);
        }

        await _repository.AtualizarAsync(usuario);

        var response = new AtualizarUsuarioResponse
        {
            Id = usuario.Id,
            Email = usuario.Email
        };

        return ResultViewModel<AtualizarUsuarioResponse>.Success(response);
    }
}
