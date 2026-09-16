using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Usuarios.RemoverUsuario;

public class RemoverUsuarioHandler(IUsuarioRepository _repository) : IRequestHandler<RemoverUsuarioCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(RemoverUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuario = await _repository.ObterPorIdAsync(command.Id);

        if (usuario == null)
            return ResultViewModel<bool>.Error("Usuário não encontrado.");

        await _repository.RemoverAsync(usuario);

        return ResultViewModel<bool>.Success(true);
    }
}
