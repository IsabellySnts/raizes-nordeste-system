using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Unidades.AtualizarUnidade;
using RaizesDoNordeste.Application.Commands.Unidades.CriarUnidade;
using RaizesDoNordeste.Application.Commands.Unidades.RemoverUnidade;
using RaizesDoNordeste.Application.Queries.Unidades.ObterTodosUnidadesQuery;
using RaizesDoNordeste.Application.Queries.Unidades.ObterUnidadePorId;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
[SwaggerTag("Gerenciamento das unidades (franquias) da rede")]
public class UnidadeController : BaseController
{
    private readonly IMediator _mediator;

    public UnidadeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Cadastrar unidade",
        Description = "Cadastra uma nova unidade (franquia) da rede com nome, endereço e dados de operação. Apenas Admin.")]
    [SwaggerResponse(201, "Unidade cadastrada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin")]
    public async Task<IActionResult> Criar([FromBody] CriarUnidadeCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Gerente, Cliente, Atendente")]
    [SwaggerOperation(
        Summary = "Obter unidade por ID",
        Description = "Retorna os dados de uma unidade específica da rede.")]
    [SwaggerResponse(200, "Unidade encontrada")]
    [SwaggerResponse(404, "Unidade não encontrada")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterUnidadePorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gerente, Cliente, Atendente")]
    [SwaggerOperation(
        Summary = "Listar todas as unidades",
        Description = "Retorna a lista completa de unidades da rede.")]
    [SwaggerResponse(200, "Lista de unidades retornada")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterTodas()
    {
        var response = await _mediator.Send(new ObterTodasUnidadesQuery());

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Atualizar unidade",
        Description = "Atualiza os dados de uma unidade existente (nome, endereço, horário de funcionamento). Apenas Admin.")]
    [SwaggerResponse(200, "Unidade atualizada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin")]
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
    [SwaggerOperation(
        Summary = "Remover unidade",
        Description = "Remove uma unidade da rede. Unidades com pedidos ativos ou funcionários vinculados podem não ser elegíveis para remoção. Apenas Admin.")]
    [SwaggerResponse(204, "Unidade removida com sucesso")]
    [SwaggerResponse(400, "Unidade não pode ser removida (vínculos ativos)")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin")]
    public async Task<IActionResult> Remover(long id)
    {
        var response = await _mediator.Send(new RemoverUnidadeCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return NoContent();
    }
}