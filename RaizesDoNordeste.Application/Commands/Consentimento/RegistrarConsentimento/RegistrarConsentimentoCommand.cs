using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;

public sealed record RegistrarConsentimentoCommand : IRequest<ResultViewModel<RegistrarConsentimentoResponse>>
{
    public long IdCliente { get; init; }
    public TipoConsentimento Permissao { get; init; }
    public bool Aceite { get; init; }
}
