using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.CriarFuncionario;

public sealed record CriarFuncionarioCommand : IRequest<ResultViewModel<CriarFuncionarioResponse>>
{
    public string Nome { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public long? IdUnidade { get; init; }
    public CargoFuncionario Cargo { get; init; }
    public string Senha { get; init; } = string.Empty;
    public string ConfirmarSenha { get; init; } = string.Empty;
}
