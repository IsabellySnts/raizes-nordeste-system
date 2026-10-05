using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Cardapios.AtualizarCardapio;
using RaizesDoNordeste.Application.Commands.Cardapios.DesvincularProdutoUnidade;
using RaizesDoNordeste.Application.Commands.Cardapios.VincularProdutoUnidade;
using RaizesDoNordeste.Application.Queries.Cardapios.ObterCardapioUnidade;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento do cardápio por unidade")]
public class CardapioController(IMediator _mediator) : BaseController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Obter cardápio da unidade", Description = "Retorna os produtos disponíveis no cardápio de uma unidade. Pode filtrar apenas os disponíveis.")]
    [SwaggerResponse(200, "Cardápio retornado com sucesso")]
    [SwaggerResponse(404, "Unidade não encontrada")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterCardapio(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [FromQuery, SwaggerParameter("Filtrar apenas produtos disponíveis")] bool apenasDisponiveis = true)
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
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Vincular produto ao cardápio", Description = "Adiciona um produto ao cardápio de uma unidade, definindo preço e disponibilidade. Acesso restrito a Admin e Gerente.")]
    [SwaggerResponse(201, "Produto vinculado ao cardápio com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> VincularProduto(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [FromBody] VincularProdutoCommand command)
    {
        if (unidadeId != command.IdUnidade)
            return BadRequest(new { error = "O Id da unidade na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Created($"api/unidades/{unidadeId}/cardapio", response.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Atualizar item do cardápio", Description = "Atualiza preço, disponibilidade ou outras informações de um item do cardápio.")]
    [SwaggerResponse(200, "Item atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Atualizar(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [SwaggerParameter("ID do item no cardápio")] long id,
        [FromBody] AtualizarCardapioCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Desvincular produto do cardápio", Description = "Remove um produto do cardápio de uma unidade.")]
    [SwaggerResponse(204, "Produto desvinculado com sucesso")]
    [SwaggerResponse(400, "Erro ao desvincular")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Desvincular(
        [SwaggerParameter("ID da unidade")] long unidadeId,
        [SwaggerParameter("ID do item no cardápio")] long id)
    {
        var response = await _mediator.Send(new DesvincularProdutoCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return NoContent();
    }
}