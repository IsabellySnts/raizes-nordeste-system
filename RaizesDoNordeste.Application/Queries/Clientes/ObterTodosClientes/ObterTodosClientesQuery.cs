using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Clientes.ObterTodosClientes;

public sealed record ObterTodosClientesQuery : IRequest<ResultViewModel<IEnumerable<ClienteQueryResponse>>>
{
}
