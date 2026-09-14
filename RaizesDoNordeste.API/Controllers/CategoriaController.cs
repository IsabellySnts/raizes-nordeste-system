using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;

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
}
