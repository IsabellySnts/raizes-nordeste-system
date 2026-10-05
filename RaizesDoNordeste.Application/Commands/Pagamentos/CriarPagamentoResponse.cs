using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Pagamentos;

public class CriarPagamentoResponse
{
    public long IdPagamento { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public StatusPagamento Status { get; set; }
}