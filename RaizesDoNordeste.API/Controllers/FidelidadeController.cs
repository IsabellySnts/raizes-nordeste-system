using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;
using RaizesDoNordeste.Application.Commands.Fidelidades.AderirFidelidade;
using RaizesDoNordeste.Application.Queries.Fidelidades.ObterExtratoFidelidade;
using RaizesDoNordeste.Application.Queries.Fidelidades.ObterFidelidadeCliente;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Programa de fidelidade — acúmulo e resgate de pontos")]
public class FidelidadeController(IMediator _mediator) : BaseController
{
    [HttpPost("aderir")]
    [Authorize(Roles = "Cliente")]
    [SwaggerOperation(Summary = "Aderir ao programa de fidelidade", Description = "Cadastra o cliente no programa de fidelidade. A partir da adesão, ele passa a acumular pontos a cada compra (R$1 = 1 ponto).")]
    [SwaggerResponse(201, "Adesão realizada com sucesso")]
    [SwaggerResponse(400, "Cliente já aderiu ou dados inválidos")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer perfil Cliente)")]
    public async Task<IActionResult> Aderir([SwaggerParameter("ID do cliente")] long clienteId)
    {
        var response = await _mediator.Send(new AderirFidelidadeCommand { IdCliente = clienteId });

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return CreatedAtAction(nameof(Consultar), new { id = response.Data!.Id }, response.Data);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
    [SwaggerOperation(Summary = "Consultar saldo de fidelidade", Description = "Retorna o saldo atual de pontos do cliente no programa de fidelidade.")]
    [SwaggerResponse(200, "Saldo retornado com sucesso")]
    [SwaggerResponse(404, "Cliente não encontrado no programa de fidelidade")]
    [SwaggerResponse(401, "Não autenticado")]
    public async Task<IActionResult> Consultar([SwaggerParameter("ID do cliente")] long clienteId)
    {
        var response = await _mediator.Send(new ObterFidelidadeClienteQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpGet("extrato")]
    [Authorize(Roles = "Admin,Gerente,Cliente")]
    [SwaggerOperation(Summary = "Consultar extrato de fidelidade", Description = "Retorna o histórico de acúmulos e resgates de pontos do cliente.")]
    [SwaggerResponse(200, "Extrato retornado com sucesso")]
    [SwaggerResponse(404, "Cliente não encontrado no programa de fidelidade")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão")]
    public async Task<IActionResult> Extrato([SwaggerParameter("ID do cliente")] long clienteId)
    {
        var response = await _mediator.Send(new ObterExtratoFidelidadeQuery { IdCliente = clienteId });

        if (!response.IsSuccess)
            return NotFound(new { error = response.Message });

        return Ok(response.Data);
    }

    [HttpPost("acumular")]
    [Authorize(Roles = "Admin,Gerente,Atendente")]
    [SwaggerOperation(Summary = "Acumular pontos manualmente", Description = "Adiciona pontos ao saldo do cliente. O acúmulo automático ocorre ao entregar um pedido (R$1 = 1 ponto), mas este endpoint permite acúmulo manual por funcionários.")]
    [SwaggerResponse(200, "Pontos acumulados com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
    [SwaggerResponse(401, "Não autenticado")]
    [SwaggerResponse(403, "Sem permissão (requer Admin, Gerente ou Atendente)")]
    public async Task<IActionResult> Acumular(
        [SwaggerParameter("ID do cliente")] long clienteId,
        [FromBody] AcumularPontosCommand command)
    {
        if (clienteId != command.IdCliente)
            return BadRequest(new { error = "O Id do cliente na rota não corresponde ao do corpo." });

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Message });

        return Ok(response.Data);
    }
}