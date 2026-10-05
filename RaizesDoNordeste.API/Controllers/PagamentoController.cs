using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Pagamentos;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Application.Queries.Pagamentos.ObterPagamentoPorId;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Gerenciamento de pagamentos e integração com Stripe")]
public class PagamentoController(IMediator _mediator, IPagamentoService _pagamentoService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
    [SwaggerOperation(
        Summary = "Criar pagamento",
        Description = "Cria um PaymentIntent no Stripe para o pedido informado. Retorna o client_secret para confirmação no front-end.")]
    [SwaggerResponse(200, "PaymentIntent criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou pedido não encontrado")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> Criar([FromBody] CriarPagamentoCommand command)
    {
        var resultado = await _mediator.Send(command);

        if (!resultado.IsSuccess)
            return BadRequest(new { error = resultado.Message });

        return Ok(resultado.Data);
    }

    [HttpPost("webhook")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Webhook do Stripe",
        Description = "Endpoint chamado automaticamente pelo Stripe para notificar eventos de pagamento (payment_intent.succeeded, payment_intent.payment_failed). Não deve ser chamado manualmente.")]
    [SwaggerResponse(200, "Evento processado com sucesso")]
    [SwaggerResponse(400, "Assinatura inválida ou erro no processamento")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].ToString();

        var resultado = await _pagamentoService.ProcessarWebhookAsync(json, signature);

        if (!resultado.IsSuccess)
            return BadRequest(new { error = resultado.Message });

        return Ok();
    }

    [HttpGet("pedido/{idPedido}")]
    [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
    [SwaggerOperation(
        Summary = "Consultar pagamento por pedido",
        Description = "Retorna os dados do pagamento associado ao pedido informado, incluindo status e código de transação.")]
    [SwaggerResponse(200, "Pagamento encontrado")]
    [SwaggerResponse(404, "Pagamento não encontrado para o pedido")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> ObterPorPedido(long idPedido)
    {
        var resultado = await _mediator.Send(new ObterPagamentoPorPedidoQuery(idPedido));
        if (!resultado.IsSuccess)
            return NotFound(new { error = resultado.Message });
        return Ok(resultado.Data);
    }

    [HttpPost("{idPedido}/confirmar")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(
        Summary = "Confirmar pagamento manualmente",
        Description = "Confirma o pagamento de um pedido sem depender do webhook do Stripe. Uso exclusivo para testes, apresentações ou fallback quando o webhook não está disponível.")]
    [SwaggerResponse(200, "Pagamento confirmado com sucesso")]
    [SwaggerResponse(400, "Pagamento já confirmado ou não encontrado")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin e Gerente")]
    public async Task<IActionResult> ConfirmarManualmente(long idPedido)
    {
        var pagamento = await _pagamentoService.ConfirmarManualmenteAsync(idPedido);

        if (!pagamento.IsSuccess)
            return BadRequest(new { error = pagamento.Message });

        return Ok(pagamento.Data);
    }
}