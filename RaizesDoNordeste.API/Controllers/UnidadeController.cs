using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Unidades.AtualizarUnidade;
using RaizesDoNordeste.Application.Commands.Unidades.CriarUnidade;
using RaizesDoNordeste.Application.Commands.Unidades.RemoverUnidade;
using RaizesDoNordeste.Application.Queries.Unidades.ObterTodosUnidadesQuery;
using RaizesDoNordeste.Application.Queries.Unidades.ObterUnidadePorId;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnidadeController : BaseController
{
    private readonly IMediator _mediator;

    public UnidadeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarUnidadeCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterUnidadePorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var response = await _mediator.Send(new ObterTodasUnidadesQuery());

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AtualizarUnidadeCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(long id)
    {
        var response = await _mediator.Send(new RemoverUnidadeCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return NoContent();
    }
}

