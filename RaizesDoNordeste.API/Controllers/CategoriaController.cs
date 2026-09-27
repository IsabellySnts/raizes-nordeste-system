using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Categorias.AlterarCategoria;
using RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;
using RaizesDoNordeste.Application.Queries.Categorias.ObterCategoriaPorId;
using RaizesDoNordeste.Application.Queries.Categorias.ObterTodasCategorias;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCategoriaCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(Criar), new { id = response.Data?.IdCategoria }, response);
        }
        catch (FluentValidation.ValidationException ex)
        {
            var erros = ex.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { errors = erros });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterCategoriaPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { errors = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var response = await _mediator.Send(new ObterTodasCategoriasQuery());

        if (!response.IsSuccess)
            return BadRequest(new { errors = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] AlterarCategoriaCommand command)
    {
        if (id != command.IdCategoria)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        try
        {
            var response = await _mediator.Send(command);
            return Ok(response.Data);
        }
        catch
        {
            return BadRequest(new { error = "Ocorreu um erro ao processar a solicitação." });
        }

    }
}
