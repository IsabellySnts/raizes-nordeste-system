using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Cardapios.AtualizarCardapio;
using RaizesDoNordeste.Application.Commands.Cardapios.DesvincularProdutoUnidade;
using RaizesDoNordeste.Application.Commands.Cardapios.VincularProdutoUnidade;
using RaizesDoNordeste.Application.Queries.Cardapios.ObterCardapioUnidade;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardapioController(IMediator _mediator) : BaseController
{

    [HttpGet]
    public async Task<IActionResult> ObterCardapio(long unidadeId, [FromQuery] bool apenasDisponiveis = true)
    {
        var response = await _mediator.Send(new ObterCardapioUnidadeQuery
        {
            IdUnidade = unidadeId,
            ApenasDisponiveis = apenasDisponiveis
        });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> VincularProduto(long unidadeId, [FromBody] VincularProdutoCommand command)
    {
        if (unidadeId != command.IdUnidade)
            return BadRequest(new { error = "O Id da unidade na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Created($"api/unidades/{unidadeId}/cardapio", response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long unidadeId, long id, [FromBody] AtualizarCardapioCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Desvincular(long unidadeId, long id)
    {
        var response = await _mediator.Send(new DesvincularProdutoCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return NoContent();
    }
}
