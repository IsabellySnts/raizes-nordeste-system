using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Estoques.AjustarEstoque;
using RaizesDoNordeste.Application.Commands.Estoques.CriarEstoque;
using RaizesDoNordeste.Application.Commands.Estoques.ReporEstoque;
using RaizesDoNordeste.Application.Queries.Estoques.ObterEstoqueUnidade;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Gerente,Atendente")]
[SwaggerTag("Controle de estoque de produtos por unidade")]
public class EstoqueController(IMediator _mediator) : BaseController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Consultar estoque da unidade", Description = "Retorna o estoque de todos os produtos de uma unidade. Acesso para Admin, Gerente e Atendente.")]
    [SwaggerResponse(200, "Estoque retornado com sucesso")]
    [SwaggerResponse(404, "Unidade não encontrada")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> ObterEstoque(
        [SwaggerParameter("ID da unidade")] long unidadeId)
    {
        var response = await _mediator.Send(new ObterEstoqueUnidadeQuery { IdUnidade = unidadeId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Criar registro de estoque", Description = "Cria um registro de estoque para um produto em uma unidade, definindo a quantidade inicial. Acesso restrito a Admin e Gerente.")]
    [SwaggerResponse(201, "Estoque criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Criar(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [FromBody] CriarEstoqueCommand command)
    {
        if (unidadeId != command.IdUnidade)
            return BadRequest(new { error = "O Id da unidade na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Created($"api/unidades/{unidadeId}/estoque", response.Data);
    }

    [HttpPatch("{produtoId}/repor")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Repor estoque", Description = "Adiciona quantidade ao estoque de um produto na unidade. Utilizado para registrar novas entradas de mercadoria.")]
    [SwaggerResponse(200, "Estoque reposto com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Repor(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [SwaggerParameter("ID do produto")] long produtoId,
        [FromBody] ReporEstoqueCommand command)
    {
        if (unidadeId != command.IdUnidade || produtoId != command.IdProduto)
            return BadRequest(new { error = "Os Ids da rota não correspondem aos do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPatch("{produtoId}/ajustar")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Ajustar estoque", Description = "Ajusta a quantidade do estoque para um valor específico. Utilizado para correções de inventário ou perdas.")]
    [SwaggerResponse(200, "Estoque ajustado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Ajustar(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [SwaggerParameter("ID do produto")] long produtoId,
        [FromBody] AjustarEstoqueCommand command)
    {
        if (unidadeId != command.IdUnidade || produtoId != command.IdProduto)
            return BadRequest(new { error = "Os Ids da rota não correspondem aos do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}