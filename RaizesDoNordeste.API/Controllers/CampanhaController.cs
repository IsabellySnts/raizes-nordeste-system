using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Campanhas.AlterarStatusCampanha;
using RaizesDoNordeste.Application.Commands.Campanhas.AtualizarCampanha;
using RaizesDoNordeste.Application.Commands.Campanhas.CriarCampanha;
using RaizesDoNordeste.Application.Queries.Campanhas.ObterCampanhaPorId;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CampanhaController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCampanhaCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterCampanhaPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas([FromQuery] bool apenasAtivas = false)
    {
        var response = await _mediator.Send(new ObterCampanhasQuery { ApenasAtivas = apenasAtivas });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AtualizarCampanhaCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlterarStatus(long id, [FromBody] AlterarStatusCampanhaCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }
}
