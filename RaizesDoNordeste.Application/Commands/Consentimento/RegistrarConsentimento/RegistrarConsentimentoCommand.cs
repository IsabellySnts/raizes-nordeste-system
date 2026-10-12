using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using System.Text.Json.Serialization;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;

public sealed record RegistrarConsentimentoCommand : IRequest<ResultViewModel<RegistrarConsentimentoResponse>>
{
    public long IdCliente { get; init; }
    public TipoConsentimento Permissao { get; init; }
    [JsonIgnore]
    public long IdUsuarioLogado { get; init; }

    [JsonIgnore]
    public PerfilUsuario PerfilLogado { get; init; }
}
