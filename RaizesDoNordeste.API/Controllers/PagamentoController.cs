using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Pagamentos;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Application.Queries.Pagamentos.ObterPagamentoPorId;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagamentoController(IMediator _mediator, IPagamentoService _pagamentoService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente,Atendente, Cliente")]
    public async Task<IActionResult> Criar([FromBody] CriarPagamentoCommand command)
    {
        var resultado = await _mediator.Send(command);

        if (!resultado.IsSuccess)
            return BadRequest(new { error = resultado.Message });

        return Ok(resultado.Data);
    }

    [HttpPost("webhook")]
    [AllowAnonymous]
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
    public async Task<IActionResult> ObterPorPedido(long idPedido)
    {
        var resultado = await _mediator.Send(new ObterPagamentoPorPedidoQuery(idPedido));
        if (!resultado.IsSuccess)
            return NotFound(new { error = resultado.Message });
        return Ok(resultado.Data);
    }

    [HttpPost("{idPedido}/confirmar")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<IActionResult> ConfirmarManualmente(long idPedido)
    {
        var pagamento = await _pagamentoService.ConfirmarManualmenteAsync(idPedido);

        if (!pagamento.IsSuccess)
            return BadRequest(new { error = pagamento.Message });

        return Ok(pagamento.Data);
    }
}
