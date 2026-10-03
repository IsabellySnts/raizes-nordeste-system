using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;
using RaizesDoNordeste.Application.Commands.Fidelidades.AderirFidelidade;
using RaizesDoNordeste.Application.Commands.Fidelidades.ResgatarPontos;
using RaizesDoNordeste.Application.Queries.Categorias.ObterCategoriaPorId;
using RaizesDoNordeste.Application.Queries.Fidelidades.ObterExtratoFidelidade;
using RaizesDoNordeste.Application.Queries.Fidelidades.ObterFidelidadeCliente;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class FidelidadeController(IMediator _mediator) : BaseController
{
    [Authorize(Roles = "Cliente")]
    [HttpPost("aderir")]
    public async Task<IActionResult> Aderir(long clienteId)
    {
        var response = await _mediator.Send(new AderirFidelidadeCommand { IdCliente = clienteId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(Consultar), new { id = response.Data!.Id }, response.Data);
    }

    [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
    [HttpGet]
    public async Task<IActionResult> Consultar(long clienteId)
    {
        var response = await _mediator.Send(new ObterFidelidadeClienteQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [Authorize(Roles = "Admin,Gerente,Cliente")]
    [HttpGet("extrato")]
    public async Task<IActionResult> Extrato(long clienteId)
    {
        var response = await _mediator.Send(new ObterExtratoFidelidadeQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [Authorize(Roles = "Admin,Gerente,Atendente")]
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

    [Authorize(Roles = "Cliente")]
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
