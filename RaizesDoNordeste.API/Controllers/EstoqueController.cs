using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Estoques.AjustarEstoque;
using RaizesDoNordeste.Application.Commands.Estoques.CriarEstoque;
using RaizesDoNordeste.Application.Commands.Estoques.ReporEstoque;
using RaizesDoNordeste.Application.Queries.Estoques.ObterEstoqueUnidade;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstoqueController(IMediator _mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> ObterEstoque(long unidadeId)
    {
        var response = await _mediator.Send(new ObterEstoqueUnidadeQuery { IdUnidade = unidadeId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(long unidadeId, [FromBody] CriarEstoqueCommand command)
    {
        if (unidadeId != command.IdUnidade)
            return BadRequest(new { error = "O Id da unidade na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Created($"api/unidades/{unidadeId}/estoque", response.Data);
    }

    [HttpPatch("{produtoId}/repor")]
    public async Task<IActionResult> Repor(long unidadeId, long produtoId, [FromBody] ReporEstoqueCommand command)
    {
        if (unidadeId != command.IdUnidade || produtoId != command.IdProduto)
            return BadRequest(new { error = "Os Ids da rota não correspondem aos do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPatch("{produtoId}/ajustar")]
    public async Task<IActionResult> Ajustar(long unidadeId, long produtoId, [FromBody] AjustarEstoqueCommand command)
    {
        if (unidadeId != command.IdUnidade || produtoId != command.IdProduto)
            return BadRequest(new { error = "Os Ids da rota não correspondem aos do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}
