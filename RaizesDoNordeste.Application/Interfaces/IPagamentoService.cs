using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Interfaces;

public interface IPagamentoService
{
    Task<ResultViewModel<PagamentoCriadoResult>> CriarPagamentoAsync(long idPedido, decimal valor, TipoPagamento tipo);
    Task<ResultViewModel<PagamentoConfirmadoResult>> ProcessarWebhookAsync(string json, string stripeSignature);
    Task<ResultViewModel<PagamentoConfirmadoResult>> ConfirmarManualmenteAsync(long idPedido);
}

public record PagamentoCriadoResult(long IdPagamento, string ClientSecret, string StripePaymentIntentId);
public record PagamentoConfirmadoResult(long IdPedido, string Status, string CodigoTransacao);