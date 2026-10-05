using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Clientes.AnonimizarCliente;
using RaizesDoNordeste.Application.Commands.Clientes.AtualizarCliente;
using RaizesDoNordeste.Application.Commands.Clientes.CriarCliente;
using RaizesDoNordeste.Application.Queries.Clientes.ObterClientePorId;
using RaizesDoNordeste.Application.Queries.Clientes.ObterTodosClientes;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Gerenciamento de clientes e conformidade LGPD")]
public class ClienteController : BaseController
{
    private readonly IMediator _mediator;

    public ClienteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Cadastrar cliente", Description = "Registra um novo cliente com consentimentos LGPD obrigatórios (2 = Dados Pessoais e 6 = Termos de Uso). Endpoint público.")]
    [SwaggerResponse(201, "Cliente cadastrado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou consentimentos obrigatórios ausentes")]
    public async Task<IActionResult> Criar([FromBody] CriarClienteCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Gerente,Atendente, Cliente")]
    [SwaggerOperation(Summary = "Obter cliente por ID", Description = "Retorna os dados de um cliente específico. Acesso restrito a Admin, Gerente e Atendente.")]
    [SwaggerResponse(200, "Cliente encontrado")]
    [SwaggerResponse(404, "Cliente não encontrado")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> ObterPorId([SwaggerParameter("ID do cliente")] long id)
    {
        var response = await _mediator.Send(new ObterClientePorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Listar clientes", Description = "Retorna todos os clientes cadastrados. Acesso restrito a Admin e Gerente.")]
    [SwaggerResponse(200, "Lista de clientes retornada com sucesso")]
    [SwaggerResponse(400, "Erro na consulta")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> ObterTodos()
    {
        var response = await _mediator.Send(new ObterTodosClientesQuery());

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Gerente, Cliente")]
    [SwaggerOperation(Summary = "Atualizar cliente", Description = "Atualiza os dados de um cliente existente.")]
    [SwaggerResponse(200, "Cliente atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Atualizar(
        [SwaggerParameter("ID do cliente")] long id,
        [FromBody] AtualizarClienteCommand command)
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
    [SwaggerOperation(Summary = "Anonimizar cliente (LGPD)", Description = "Anonimiza os dados pessoais do cliente em conformidade com a LGPD. Os dados são substituídos por valores genéricos, mantendo a integridade referencial.")]
    [SwaggerResponse(200, "Cliente anonimizado com sucesso")]
    [SwaggerResponse(400, "Erro ao anonimizar")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Gerente)")]
    public async Task<IActionResult> Anonimizar([SwaggerParameter("ID do cliente")] long id)
    {
        var response = await _mediator.Send(new AnonimizarClienteCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }
}