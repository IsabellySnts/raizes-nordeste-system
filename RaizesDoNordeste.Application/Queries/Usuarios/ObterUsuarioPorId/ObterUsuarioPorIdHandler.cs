using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Usuarios.ObterUsuarioPorId;

public class ObterUsuarioPorIdHandler(IUsuarioRepository _repository) : IRequestHandler<ObterUsuarioPorIdQuery, ResultViewModel<UsuarioQueryResponse>>
{
    public async Task<ResultViewModel<UsuarioQueryResponse>> Handle(ObterUsuarioPorIdQuery query, CancellationToken cancellationToken)
    {
        var usuario = await _repository.ObterPorIdAsync(query.Id);

        if (usuario == null)
            return ResultViewModel<UsuarioQueryResponse>.Error("Usuário não encontrado.");

        var response = new UsuarioQueryResponse
        {
            Id = usuario.Id,
            Email = usuario.Email
        };

        return ResultViewModel<UsuarioQueryResponse>.Success(response);
    }
}