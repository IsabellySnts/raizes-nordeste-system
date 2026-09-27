using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;
using RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;
using RaizesDoNordeste.Application.Queries.ConsentimentoLGPD.ObterConsentimentoCliente;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsentimentoController (IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Registrar(long clienteId, [FromBody] RegistrarConsentimentoCommand command)
    {
        if (clienteId != command.IdCliente)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorCliente), new { clienteId }, response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterPorCliente(long clienteId)
    {
        var response = await _mediator.Send(new ObterConsentimentosClienteQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpDelete("{permissao}")]
    public async Task<IActionResult> Revogar(long clienteId, TipoConsentimento permissao)
    {
        var response = await _mediator.Send(new RevogarConsentimentoCommand
        {
            IdCliente = clienteId,
            Permissao = permissao
        });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}
