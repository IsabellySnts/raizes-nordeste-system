using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Funcionarios.ObterTodosFuncionarios;

public sealed record ObterTodosFuncionariosQuery : IRequest<ResultViewModel<IEnumerable<FuncionarioQueryResponse>>>
{
    public long? IdUnidade { get; init; }
}
