using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Queries.Auditorias.ObterAuditorias;
using RaizesDoNordeste.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
[SwaggerTag("Gerenciamento de logs de auditoria do sistema")]
public class AuditoriaController(IMediator _mediator) : BaseController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Listar auditorias", Description = "Retorna os registros de auditoria do sistema. Permite filtrar por funcionário, ação, tipo de entidade e ID da entidade. Acesso restrito a administradores.")]
    [SwaggerResponse(200, "Lista de auditorias retornada com sucesso")]
    [SwaggerResponse(400, "Erro na consulta")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer perfil Admin)")]
    public async Task<IActionResult> ObterTodos(
        [FromQuery, SwaggerParameter("ID do funcionário para filtrar")] long? funcionarioId,
        [FromQuery, SwaggerParameter("Tipo de ação realizada")] AcaoAuditoria? acao,
        [FromQuery, SwaggerParameter("Tipo da entidade afetada (ex: Pedido, Produto)")] string? tipoEntidade,
        [FromQuery, SwaggerParameter("ID da entidade afetada")] long? idEntidade)
    {
        var response = await _mediator.Send(new ObterAuditoriasQuery
        {
            IdFuncionario = funcionarioId,
            Acao = acao,
            TipoEntidade = tipoEntidade,
            IdEntidade = idEntidade
        });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}
