using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.AtivarFuncionario;

public sealed record AtivarFuncionarioCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}