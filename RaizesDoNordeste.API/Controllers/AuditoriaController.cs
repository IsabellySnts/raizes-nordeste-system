using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Queries.Auditorias.ObterAuditorias;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuditoriaController(IMediator _mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> ObterTodos(
        [FromQuery] long? funcionarioId,
        [FromQuery] AcaoAuditoria? acao,
        [FromQuery] string? tipoEntidade,
        [FromQuery] long? idEntidade)
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
