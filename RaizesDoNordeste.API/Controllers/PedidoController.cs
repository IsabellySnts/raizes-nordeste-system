using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Pedidos.AtualizarStatusPedido;
using RaizesDoNordeste.Application.Commands.Pedidos.CancelarPedido;
using RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;
using RaizesDoNordeste.Application.Queries.Pedidos.ObterFilaCozinha;
using RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidoPorId;
using RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidosPorUnidade;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController(IMediator _mediator) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var response = await _mediator.Send(new ObterPedidoPorIdQuery { Id = id });

            if (!response.IsSuccess)
                return NotFound(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpGet("unidade/{unidadeId}")]
        public async Task<IActionResult> ObterPorUnidade(long unidadeId)
        {
            var response = await _mediator.Send(new ObterPedidosPorUnidadeQuery { IdUnidade = unidadeId });

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpGet("unidade/{unidadeId}/cozinha")]
        public async Task<IActionResult> FilaCozinha(long unidadeId)
        {
            var response = await _mediator.Send(new ObterFilaCozinhaQuery { IdUnidade = unidadeId });

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(long id, [FromBody] AtualizarStatusPedidoCommand command)
        {
            if (id != command.IdPedido)
                return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(long id, [FromBody] CancelarPedidoCommand command)
        {
            if (id != command.IdPedido)
                return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }
    }
}
