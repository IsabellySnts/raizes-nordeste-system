using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Pedidos.AtualizarStatusPedido;
using RaizesDoNordeste.Application.Commands.Pedidos.CancelarPedido;
using RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;
using RaizesDoNordeste.Application.Queries.Pedidos.ObterFilaCozinha;
using RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidoPorId;
using RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidosPorUnidade;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Gerenciamento de pedidos — criação, consulta, fila da cozinha e controle de status")]
    public class PedidoController(IMediator _mediator) : BaseController
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Gerente,Atendente")]
        [SwaggerOperation(
            Summary = "Criar pedido",
            Description = "Registra um novo pedido com os itens selecionados para uma unidade. O pedido é criado com status 'Pendente'.")]
        [SwaggerResponse(201, "Pedido criado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou produto indisponível")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão")]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter pedido por ID",
            Description = "Retorna os dados completos de um pedido, incluindo itens, status e informações de pagamento.")]
        [SwaggerResponse(200, "Pedido encontrado")]
        [SwaggerResponse(404, "Pedido não encontrado")]
        [SwaggerResponse(401, "Não autenticado")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var response = await _mediator.Send(new ObterPedidoPorIdQuery { Id = id });

            if (!response.IsSuccess)
                return NotFound(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpGet("unidade/{unidadeId}")]
        [Authorize(Roles = "Admin,Gerente,Atendente")]
        [SwaggerOperation(
            Summary = "Listar pedidos por unidade",
            Description = "Retorna todos os pedidos de uma unidade específica da rede.")]
        [SwaggerResponse(200, "Lista de pedidos da unidade")]
        [SwaggerResponse(400, "Unidade inválida")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão")]
        public async Task<IActionResult> ObterPorUnidade(long unidadeId)
        {
            var response = await _mediator.Send(new ObterPedidosPorUnidadeQuery { IdUnidade = unidadeId });

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpGet("unidade/{unidadeId}/cozinha")]
        [Authorize(Roles = "Admin,Gerente,Atendente,Cozinheiro")]
        [SwaggerOperation(
            Summary = "Fila da cozinha",
            Description = "Retorna os pedidos pendentes de preparo para a cozinha de uma unidade, ordenados por prioridade e horário.")]
        [SwaggerResponse(200, "Fila da cozinha retornada")]
        [SwaggerResponse(400, "Unidade inválida")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão")]
        public async Task<IActionResult> FilaCozinha(long unidadeId)
        {
            var response = await _mediator.Send(new ObterFilaCozinhaQuery { IdUnidade = unidadeId });

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Gerente,Atendente,Cozinheiro")]
        [SwaggerOperation(
            Summary = "Atualizar status do pedido",
            Description = "Altera o status de um pedido (ex.: Pendente → EmPreparo → Pronto → Entregue). O Cozinheiro utiliza este endpoint para avançar o preparo.")]
        [SwaggerResponse(200, "Status atualizado com sucesso")]
        [SwaggerResponse(400, "Transição de status inválida ou IDs não correspondem")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão")]
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
        [Authorize(Roles = "Admin,Gerente")]
        [SwaggerOperation(
            Summary = "Cancelar pedido",
            Description = "Cancela um pedido existente. Apenas pedidos com status Pendente ou Pago podem ser cancelados — pedidos já em preparo ou entregues não são elegíveis. Requer perfil Admin ou Gerente.")]
        [SwaggerResponse(200, "Pedido cancelado com sucesso")]
        [SwaggerResponse(400, "Pedido não pode ser cancelado ou IDs não correspondem")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão — apenas Admin e Gerente")]
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