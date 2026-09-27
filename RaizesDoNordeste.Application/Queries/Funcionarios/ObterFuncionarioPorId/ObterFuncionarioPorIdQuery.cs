using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Funcionarios.ObterFuncionarioPorId;

public sealed record ObterFuncionarioPorIdQuery : IRequest<ResultViewModel<FuncionarioQueryResponse>>
{
    public long Id { get; init; }
}
