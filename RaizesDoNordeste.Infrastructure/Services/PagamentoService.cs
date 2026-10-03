using Microsoft.Extensions.Options;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;
using RaizesDoNordeste.Infrastructure.Settings;
using Stripe;

namespace RaizesDoNordeste.Infrastructure.Services;

public class PagamentoService : IPagamentoService
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly string _webhookSecret;

    public PagamentoService(
        IPagamentoRepository pagamentoRepository,
        IPedidoRepository pedidoRepository,
        IOptions<StripeSettings> stripeSettings)
    {
        _pagamentoRepository = pagamentoRepository;
        _pedidoRepository = pedidoRepository;
        _webhookSecret = stripeSettings.Value.WebhookSecret;

        StripeConfiguration.ApiKey = stripeSettings.Value.SecretKey;
    }

    public async Task<ResultViewModel<PagamentoCriadoResult>> CriarPagamentoAsync(
        long idPedido, decimal valor, TipoPagamento tipo)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(idPedido);
        if (pedido == null)
            return ResultViewModel<PagamentoCriadoResult>.Error("Pedido não encontrado.");

        if (pedido.Status != StatusPedido.AguardandoPagamento)
            return ResultViewModel<PagamentoCriadoResult>.Error("Pedido não está aguardando pagamento.");

        var paymentMethods = MapearMetodosPagamento(tipo);

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(valor * 100),
            Currency = "brl",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { "idPedido", idPedido.ToString() },
                { "tipoPagamento", tipo.ToString() }
            }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        var pagamento = new Pagamento(idPedido, valor, tipo);
        pagamento.IncrementarTentativa();
        var pagamentoCriado = await _pagamentoRepository.CriarAsync(pagamento);

        var result = new PagamentoCriadoResult(
            pagamentoCriado.Id,
            paymentIntent.ClientSecret,
            paymentIntent.Id
        );

        return ResultViewModel<PagamentoCriadoResult>.Success(result);
    }

    public async Task<ResultViewModel<PagamentoConfirmadoResult>> ProcessarWebhookAsync(
        string json, string stripeSignature)
    {
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);
        }
        catch (StripeException)
        {
            return ResultViewModel<PagamentoConfirmadoResult>.Error("Assinatura do webhook inválida.");
        }

        if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
                return ResultViewModel<PagamentoConfirmadoResult>.Error("PaymentIntent inválido.");

            var idPedido = long.Parse(paymentIntent.Metadata["idPedido"]);

            var pagamento = await _pagamentoRepository.ObterPorPedidoIdAsync(idPedido);
            if (pagamento == null)
                return ResultViewModel<PagamentoConfirmadoResult>.Error("Pagamento não encontrado.");

            pagamento.RegistrarRetorno(StatusPagamento.Aprovado, paymentIntent.Id);
            await _pagamentoRepository.AtualizarAsync(pagamento);

            var pedido = await _pedidoRepository.ObterPorIdAsync(idPedido);
            if (pedido != null)
            {
                pedido.AtualizarStatus(StatusPedido.Pago);
                await _pedidoRepository.AtualizarAsync(pedido);
            }

            var result = new PagamentoConfirmadoResult(idPedido, "Aprovado", paymentIntent.Id);
            return ResultViewModel<PagamentoConfirmadoResult>.Success(result);
        }

        if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
                return ResultViewModel<PagamentoConfirmadoResult>.Error("PaymentIntent inválido.");

            var idPedido = long.Parse(paymentIntent.Metadata["idPedido"]);

            var pagamento = await _pagamentoRepository.ObterPorPedidoIdAsync(idPedido);
            if (pagamento != null)
            {
                pagamento.RegistrarRetorno(StatusPagamento.Recusado, paymentIntent.Id);
                await _pagamentoRepository.AtualizarAsync(pagamento);
            }

            var result = new PagamentoConfirmadoResult(idPedido, "Recusado", paymentIntent.Id);
            return ResultViewModel<PagamentoConfirmadoResult>.Success(result);
        }

        return ResultViewModel<PagamentoConfirmadoResult>.Success(
            new PagamentoConfirmadoResult(0, "Evento ignorado", ""));
    }

    public async Task<ResultViewModel<PagamentoConfirmadoResult>> ConfirmarManualmenteAsync(long idPedido)
    {
        var pagamento = await _pagamentoRepository.ObterPorPedidoIdAsync(idPedido);
        if (pagamento is null)
            return ResultViewModel<PagamentoConfirmadoResult>.Error("Pagamento não encontrado para este pedido.");

        if (pagamento.Status == StatusPagamento.Aprovado)
            return ResultViewModel<PagamentoConfirmadoResult>.Error("Pagamento já foi confirmado.");

        pagamento.RegistrarRetorno(StatusPagamento.Aprovado, $"MANUAL-{DateTime.UtcNow:yyyyMMddHHmmss}");
        await _pagamentoRepository.AtualizarAsync(pagamento);

        var pedido = await _pedidoRepository.ObterPorIdAsync(idPedido);
        if (pedido is not null)
        {
            pedido.AtualizarStatus(StatusPedido.Pago);
            await _pedidoRepository.AtualizarAsync(pedido);
        }

        var result = new PagamentoConfirmadoResult(idPedido, "Aprovado", pagamento.CodigoTransacao ?? "MANUAL");
        return ResultViewModel<PagamentoConfirmadoResult>.Success(result);
    }

    private static List<string> MapearMetodosPagamento(TipoPagamento tipo)
    {
        return tipo switch
        {
            TipoPagamento.CartaoCredito => new List<string> { "card" },
            TipoPagamento.CartaoDebito => new List<string> { "card" },
            TipoPagamento.Pix => new List<string> { "pix" },
            _ => new List<string> { "card" }
        };
    }
}
