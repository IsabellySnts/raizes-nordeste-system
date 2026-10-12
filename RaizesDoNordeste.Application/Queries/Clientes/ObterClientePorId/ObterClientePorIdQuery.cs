using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Queries.Clientes.ObterClientePorId;

public sealed record ObterClientePorIdQuery : IRequest<ResultViewModel<ClienteQueryResponse>>
{
    public long Id { get; init; }
    public long IdUsuarioLogado { get; init; }
    public PerfilUsuario PerfilLogado { get; init; }
}
