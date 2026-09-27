using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Usuarios.CriarUsuario;

public class CriarUsuarioHandler(IUsuarioRepository _repository) : IRequestHandler<CriarUsuarioCommand, ResultViewModel<CriarUsuarioResponse>>
{
    public async Task<ResultViewModel<CriarUsuarioResponse>> Handle(
        CriarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarUsuarioCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarUsuarioResponse>.Error(erros);
        }

        var emailExiste = await _repository.EmailExisteAsync(command.Email);
        if (emailExiste)
            return ResultViewModel<CriarUsuarioResponse>.Error("Este email já está cadastrado.");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(command.Senha);
        var usuario = new Usuario(command.Email, senhaHash);

        var usuarioCriado = await _repository.CriarAsync(usuario);

        var response = new CriarUsuarioResponse
        {
            Id = usuarioCriado.Id,
            Email = usuarioCriado.Email
        };

        return ResultViewModel<CriarUsuarioResponse>.Success(response);
    }
}