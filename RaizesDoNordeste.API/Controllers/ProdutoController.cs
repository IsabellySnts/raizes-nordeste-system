using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Produtos.AlterarProduto;
using RaizesDoNordeste.Application.Commands.Produtos.CriarProduto;
using RaizesDoNordeste.Application.Commands.Produtos.RemoverProduto;
using RaizesDoNordeste.Application.Queries.Produtos.ObterProdutoPorId;
using RaizesDoNordeste.Application.Queries.Produtos.ObterTodosProdutos;

namespace RaizesDoNordeste.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterProdutoPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var response = await _mediator.Send(new ObterTodosProdutosQuery());

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AlterarProdutoCommand command)
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
        var response = await _mediator.Send(new RemoverProdutoCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return NoContent();
    }
}
