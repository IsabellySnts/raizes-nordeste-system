using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Clientes.AnonimizarCliente;
using RaizesDoNordeste.Application.Commands.Clientes.AtualizarCliente;
using RaizesDoNordeste.Application.Commands.Clientes.CriarCliente;
using RaizesDoNordeste.Application.Queries.Clientes.ObterClientePorId;
using RaizesDoNordeste.Application.Queries.Clientes.ObterTodosClientes;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClienteController : BaseController
{
    private readonly IMediator _mediator;

    public ClienteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterClientePorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var response = await _mediator.Send(new ObterTodosClientesQuery());

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AtualizarClienteCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Anonimizar(long id)
    {
        var response = await _mediator.Send(new AnonimizarClienteCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }
}
