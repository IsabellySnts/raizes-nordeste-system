using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;
using RaizesDoNordeste.Application.Commands.Fidelidades.AderirFidelidade;
using RaizesDoNordeste.Application.Commands.Fidelidades.ResgatarPontos;
using RaizesDoNordeste.Application.Queries.Fidelidades.ObterExtratoFidelidade;
using RaizesDoNordeste.Application.Queries.Fidelidades.ObterFidelidadeCliente;

namespace RaizesDoNordeste.API.Controllers;

public class FidelidadeController(IMediator _mediator) : BaseController
{
    [HttpPost("aderir")]
    public async Task<IActionResult> Aderir(long clienteId)
    {
        var response = await _mediator.Send(new AderirFidelidadeCommand { IdCliente = clienteId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Created($"api/clientes/{clienteId}/fidelidade", response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Consultar(long clienteId)
    {
        var response = await _mediator.Send(new ObterFidelidadeClienteQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet("extrato")]
    public async Task<IActionResult> Extrato(long clienteId)
    {
        var response = await _mediator.Send(new ObterExtratoFidelidadeQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPost("acumular")]
    public async Task<IActionResult> Acumular(long clienteId, [FromBody] AcumularPontosCommand command)
    {
        if (clienteId != command.IdCliente)
            return BadRequest(new { error = "O Id do cliente na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPost("resgatar")]
    public async Task<IActionResult> Resgatar(long clienteId, [FromBody] ResgatarPontosCommand command)
    {
        if (clienteId != command.IdCliente)
            return BadRequest(new { error = "O Id do cliente na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}
