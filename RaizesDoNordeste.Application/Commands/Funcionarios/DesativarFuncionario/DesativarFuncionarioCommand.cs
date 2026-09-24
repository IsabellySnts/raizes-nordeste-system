using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.DesativarFuncionario;

public sealed record DesativarFuncionarioCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}
