using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Funcionarios.AtivarFuncionario;
using RaizesDoNordeste.Application.Commands.Funcionarios.AtualizarFuncionario;
using RaizesDoNordeste.Application.Commands.Funcionarios.CriarFuncionario;
using RaizesDoNordeste.Application.Commands.Funcionarios.DesativarFuncionario;
using RaizesDoNordeste.Application.Queries.Funcionarios.ObterFuncionarioPorId;
using RaizesDoNordeste.Application.Queries.Funcionarios.ObterTodosFuncionarios;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FuncionarioController(IMediator _mediator): BaseController
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarFuncionarioCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterFuncionarioPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos([FromQuery] long? unidadeId)
    {
        var response = await _mediator.Send(new ObterTodosFuncionariosQuery { IdUnidade = unidadeId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AtualizarFuncionarioCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(long id)
    {
        var response = await _mediator.Send(new DesativarFuncionarioCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }

    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(long id)
    {
        var response = await _mediator.Send(new AtivarFuncionarioCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }
}
