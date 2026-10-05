using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;
using RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;
using RaizesDoNordeste.Application.Queries.ConsentimentoLGPD.ObterConsentimentoCliente;
using RaizesDoNordeste.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Gerenciamento de consentimentos LGPD dos clientes")]
public class ConsentimentoController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin,Cliente")]
    [SwaggerOperation(Summary = "Registrar consentimento", Description = "Registra um consentimento LGPD para o cliente (ex: Dados Pessoais, Termos de Uso, Marketing).")]
    [SwaggerResponse(201, "Consentimento registrado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    public async Task<IActionResult> Registrar(
        [SwaggerParameter("ID do cliente")] long clienteId,
        [FromBody] RegistrarConsentimentoCommand command)
    {
        if (clienteId != command.IdCliente)
            return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(ObterPorCliente), new { clienteId }, response.Data);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cliente")]
    [SwaggerOperation(Summary = "Consultar consentimentos do cliente", Description = "Retorna todos os consentimentos LGPD registrados para um cliente. Acesso restrito a Admin e ao próprio Cliente.")]
    [SwaggerResponse(200, "Consentimentos retornados com sucesso")]
    [SwaggerResponse(400, "Erro na consulta")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Cliente)")]
    public async Task<IActionResult> ObterPorCliente(
        [SwaggerParameter("ID do cliente")] long clienteId)
    {
        var response = await _mediator.Send(new ObterConsentimentosClienteQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpDelete("{permissao}")]
    [Authorize(Roles = "Admin,Cliente")]
    [SwaggerOperation(Summary = "Revogar consentimento", Description = "Revoga um consentimento LGPD específico do cliente. O cliente pode revogar consentimentos opcionais (ex: Marketing), mas não os obrigatórios (Dados Pessoais, Termos de Uso).")]
    [SwaggerResponse(200, "Consentimento revogado com sucesso")]
    [SwaggerResponse(400, "Erro ao revogar (ex: consentimento obrigatório)")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin ou Cliente)")]
    public async Task<IActionResult> Revogar(
        [SwaggerParameter("ID do cliente")] long clienteId,
        [SwaggerParameter("Tipo do consentimento a revogar")] TipoConsentimento permissao)
    {
        var response = await _mediator.Send(new RevogarConsentimentoCommand
        {
            IdCliente = clienteId,
            Permissao = permissao
        });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}