using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using System.Text.Json.Serialization;

namespace RaizesDoNordeste.Application.Commands.Clientes.AtualizarCliente;

public sealed record AtualizarClienteCommand : IRequest<ResultViewModel<AtualizarClienteResponse>>
{
    public long Id { get; init; }
    [JsonIgnore]
    public long IdUsuarioLogado { get; set; }
    [JsonIgnore]
    public PerfilUsuario PerfilLogado { get; init; }
    public string NomeCompleto { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
}
