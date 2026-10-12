using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using System.Text.Json.Serialization;

namespace RaizesDoNordeste.Application.Queries.Consentimento.ObterConsentimentoCliente;

public sealed record ObterConsentimentosClienteQuery : IRequest<ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>>
{
    public long IdCliente { get; init; }
    [JsonIgnore]
    public long IdUsuarioLogado { get; init; }

    [JsonIgnore]
    public PerfilUsuario PerfilLogado { get; init; }
}
