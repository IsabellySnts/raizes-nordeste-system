using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Categorias.AlterarCategoria;
using RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;
using RaizesDoNordeste.Application.Queries.Categorias.ObterCategoriaPorId;
using RaizesDoNordeste.Application.Queries.Categorias.ObterTodasCategorias;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de categorias de produtos")]
public class CategoriaController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Criar categoria", Description = "Cadastra uma nova categoria de produtos. Acesso restrito a Admin e Gerente.")]
    [SwaggerResponse(201, "Categoria criada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
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
    [SwaggerOperation(Summary = "Obter categoria por ID", Description = "Retorna os detalhes de uma categoria específica.")]
    [SwaggerResponse(200, "Categoria encontrada")]
    [SwaggerResponse(404, "Categoria não encontrada")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterPorId([SwaggerParameter("ID da categoria")] long id)
    {
        var response = await _mediator.Send(new ObterCategoriaPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { errors = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Listar categorias", Description = "Retorna todas as categorias cadastradas.")]
    [SwaggerResponse(200, "Lista de categorias retornada com sucesso")]
    [SwaggerResponse(400, "Erro na consulta")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterTodas()
    {
        var response = await _mediator.Send(new ObterTodasCategoriasQuery());

        if (!response.IsSuccess)
            return BadRequest(new { errors = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Atualizar categoria", Description = "Atualiza os dados de uma categoria existente. O ID da rota deve corresponder ao ID do corpo.")]
    [SwaggerResponse(200, "Categoria atualizada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Atualizar(
        [SwaggerParameter("ID da categoria")] long id,
        [FromBody] AlterarCategoriaCommand command)
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