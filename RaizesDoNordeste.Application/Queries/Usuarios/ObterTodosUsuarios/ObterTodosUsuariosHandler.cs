using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Usuarios.ObterTodosUsuarios;

public class ObterTodosUsuariosHandler(IUsuarioRepository _repository) : IRequestHandler<ObterTodosUsuariosQuery, ResultViewModel<IEnumerable<UsuarioQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<UsuarioQueryResponse>>> Handle(ObterTodosUsuariosQuery query, CancellationToken cancellationToken)
    {
        var usuarios = await _repository.ObterTodosAsync();

        var response = usuarios.Select(u => new UsuarioQueryResponse
        {
            Id = u.Id,
            Email = u.Email
        });

        return ResultViewModel<IEnumerable<UsuarioQueryResponse>>.Success(response);
    }
}