using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Produtos.AlterarProduto;
using RaizesDoNordeste.Application.Commands.Produtos.CriarProduto;
using RaizesDoNordeste.Application.Commands.Produtos.RemoverProduto;
using RaizesDoNordeste.Application.Queries.Produtos.ObterProdutoPorId;
using RaizesDoNordeste.Application.Queries.Produtos.ObterTodosProdutos;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gerenciamento do catálogo de produtos da rede")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(
        Summary = "Cadastrar produto",
        Description = "Cadastra um novo produto no catálogo da rede com nome, descrição, preço e categoria.")]
    [SwaggerResponse(201, "Produto cadastrado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou categoria inexistente")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin e Gerente")]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obter produto por ID",
        Description = "Retorna os dados de um produto específico, incluindo nome, descrição, preço e categoria.")]
    [SwaggerResponse(200, "Produto encontrado")]
    [SwaggerResponse(404, "Produto não encontrado")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _mediator.Send(new ObterProdutoPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar todos os produtos",
        Description = "Retorna a lista completa de produtos cadastrados no catálogo da rede.")]
    [SwaggerResponse(200, "Lista de produtos retornada")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> ObterTodos()
    {
        var response = await _mediator.Send(new ObterTodosProdutosQuery());

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(
        Summary = "Atualizar produto",
        Description = "Atualiza os dados de um produto existente (nome, descrição, preço, categoria).")]
    [SwaggerResponse(200, "Produto atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(404, "Produto não encontrado")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin e Gerente")]
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
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(
        Summary = "Remover produto",
        Description = "Remove um produto do catálogo. Produtos vinculados a cardápios ou pedidos ativos podem não ser elegíveis para remoção.")]
    [SwaggerResponse(204, "Produto removido com sucesso")]
    [SwaggerResponse(400, "Produto não pode ser removido (vínculos ativos)")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão — apenas Admin e Gerente")]
    public async Task<IActionResult> Remover(long id)
    {
        var response = await _mediator.Send(new RemoverProdutoCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return NoContent();
    }
}