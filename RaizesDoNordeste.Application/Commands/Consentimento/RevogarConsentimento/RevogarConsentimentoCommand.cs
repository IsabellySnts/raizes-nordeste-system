using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;

public sealed record RevogarConsentimentoCommand : IRequest<ResultViewModel<RevogarConsentimentoResponse>>
{
    public long IdCliente { get; init; }
    public TipoConsentimento Permissao { get; init; }
}