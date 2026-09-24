using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.AtualizarFuncionario;

public sealed record AtualizarFuncionarioCommand : IRequest<ResultViewModel<AtualizarFuncionarioResponse>>
{
    public long Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public long? IdUnidade { get; init; }
    public CargoFuncionario Cargo { get; init; }
}
