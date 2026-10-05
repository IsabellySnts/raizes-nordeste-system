using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Funcionarios.AtivarFuncionario;
using RaizesDoNordeste.Application.Commands.Funcionarios.AtualizarFuncionario;
using RaizesDoNordeste.Application.Commands.Funcionarios.CriarFuncionario;
using RaizesDoNordeste.Application.Commands.Funcionarios.DesativarFuncionario;
using RaizesDoNordeste.Application.Queries.Funcionarios.ObterFuncionarioPorId;
using RaizesDoNordeste.Application.Queries.Funcionarios.ObterTodosFuncionarios;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
[SwaggerTag("Gerenciamento de funcionários da rede")]
public class FuncionarioController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Cadastrar funcionário", Description = "Registra um novo funcionário vinculado a uma unidade. Acesso restrito a Admin.")]
    [SwaggerResponse(201, "Funcionário cadastrado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin)")]
    public async Task<IActionResult> Criar([FromBody] CriarFuncionarioCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Obter funcionário por ID", Description = "Retorna os dados de um funcionário específico. Acesso para Admin e Gerente.")]
    [SwaggerResponse(200, "Funcionário encontrado")]
    [SwaggerResponse(404, "Funcionário não encontrado")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> ObterPorId([SwaggerParameter("ID do funcionário")] long id)
    {
        var response = await _mediator.Send(new ObterFuncionarioPorIdQuery { Id = id });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gerente")]
    [SwaggerOperation(Summary = "Listar funcionários", Description = "Retorna todos os funcionários. Pode filtrar por unidade.")]
    [SwaggerResponse(200, "Lista de funcionários retornada com sucesso")]
    [SwaggerResponse(400, "Erro na consulta")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> ObterTodos(
        [FromQuery, SwaggerParameter("Filtrar por ID da unidade")] long? unidadeId)
    {
        var response = await _mediator.Send(new ObterTodosFuncionariosQuery { IdUnidade = unidadeId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar funcionário", Description = "Atualiza os dados de um funcionário existente. Acesso restrito a Admin.")]
    [SwaggerResponse(200, "Funcionário atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin)")]
    public async Task<IActionResult> Atualizar(
        [SwaggerParameter("ID do funcionário")] long id,
        [FromBody] AtualizarFuncionarioCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPatch("{id}/desativar")]
    [SwaggerOperation(Summary = "Desativar funcionário", Description = "Desativa um funcionário sem removê-lo do sistema. O funcionário não poderá mais acessar o sistema.")]
    [SwaggerResponse(200, "Funcionário desativado com sucesso")]
    [SwaggerResponse(400, "Erro ao desativar")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin)")]
    public async Task<IActionResult> Desativar([SwaggerParameter("ID do funcionário")] long id)
    {
        var response = await _mediator.Send(new DesativarFuncionarioCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }

    [HttpPatch("{id}/ativar")]
    [SwaggerOperation(Summary = "Ativar funcionário", Description = "Reativa um funcionário previamente desativado, restaurando seu acesso ao sistema.")]
    [SwaggerResponse(200, "Funcionário ativado com sucesso")]
    [SwaggerResponse(400, "Erro ao ativar")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin)")]
    public async Task<IActionResult> Ativar([SwaggerParameter("ID do funcionário")] long id)
    {
        var response = await _mediator.Send(new AtivarFuncionarioCommand { Id = id });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(new { message = response.Message });
    }
}