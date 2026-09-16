using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Usuarios.AtualizarUsuario;
using RaizesDoNordeste.Application.Commands.Usuarios.CriarUsuario;
using RaizesDoNordeste.Application.Commands.Usuarios.RemoverUsuario;
using RaizesDoNordeste.Application.Queries.Usuarios.ObterTodosUsuarios;
using RaizesDoNordeste.Application.Queries.Usuarios.ObterUsuarioPorId;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var response = await _mediator.Send(new ObterUsuarioPorIdQuery { Id = id });

            if (!response.IsSuccess)
                return NotFound(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var response = await _mediator.Send(new ObterTodosUsuariosQuery());

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, [FromBody] AtualizarUsuarioCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { error = "O Id da rota não corresponde ao Id do corpo." });

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var response = await _mediator.Send(new RemoverUsuarioCommand { Id = id });

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return NoContent();
        }
    }
}
