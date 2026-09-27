using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Clientes.CriarCliente;

public sealed record CriarClienteCommand : IRequest<ResultViewModel<CriarClienteResponse>>
{
    public string NomeCompleto { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public string Senha { get; init; } = string.Empty;
    public string ConfirmarSenha { get; init; } = string.Empty;
}
