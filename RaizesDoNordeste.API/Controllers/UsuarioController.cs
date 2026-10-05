using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.Commands.Usuarios.AtualizarUsuario;
using RaizesDoNordeste.Application.Commands.Usuarios.CriarUsuario;
using RaizesDoNordeste.Application.Commands.Usuarios.Login;
using RaizesDoNordeste.Application.Commands.Usuarios.RemoverUsuario;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Application.Queries.Usuarios.ObterTodosUsuarios;
using RaizesDoNordeste.Application.Queries.Usuarios.ObterUsuarioPorId;
using Swashbuckle.AspNetCore.Annotations;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [SwaggerTag("Gerenciamento de usuários do sistema e autenticação")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar usuário",
            Description = "Cadastra um novo usuário no sistema com e-mail, senha e perfil (0 = Admin, 1 = Gerente, 2 = Atendente, 3 = Cozinheiro, 4 = Cliente). Apenas Admin pode criar usuários.")]
        [SwaggerResponse(201, "Usuário criado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou e-mail já cadastrado")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão — apenas Admin")]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return CreatedAtAction(nameof(ObterPorId), new { id = response.Data!.Id }, response.Data);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter usuário por ID",
            Description = "Retorna os dados de um usuário específico (sem a senha). Apenas Admin.")]
        [SwaggerResponse(200, "Usuário encontrado")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão — apenas Admin")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var response = await _mediator.Send(new ObterUsuarioPorIdQuery { Id = id });

            if (!response.IsSuccess)
                return NotFound(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todos os usuários",
            Description = "Retorna a lista completa de usuários cadastrados no sistema. Apenas Admin.")]
        [SwaggerResponse(200, "Lista de usuários retornada")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão — apenas Admin")]
        public async Task<IActionResult> ObterTodos()
        {
            var response = await _mediator.Send(new ObterTodosUsuariosQuery());

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar usuário",
            Description = "Atualiza os dados de um usuário existente (nome, e-mail, perfil). Apenas Admin.")]
        [SwaggerResponse(200, "Usuário atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou IDs não correspondem")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão — apenas Admin")]
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
        [SwaggerOperation(
            Summary = "Remover usuário",
            Description = "Remove um usuário do sistema. Usuários vinculados a funcionários ou clientes ativos podem não ser elegíveis para remoção. Apenas Admin.")]
        [SwaggerResponse(204, "Usuário removido com sucesso")]
        [SwaggerResponse(400, "Usuário não pode ser removido (vínculos ativos)")]
        [SwaggerResponse(401, "Não autenticado")]
        [SwaggerResponse(403, "Sem permissão — apenas Admin")]
        public async Task<IActionResult> Remover(long id)
        {
            var response = await _mediator.Send(new RemoverUsuarioCommand { Id = id });

            if (!response.IsSuccess)
                return BadRequest(new { error = response.Message });

            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Autenticar usuário (login)",
            Description = "Realiza a autenticação com e-mail e senha. Retorna o token JWT para uso nos endpoints protegidos. Não requer autenticação prévia.")]
        [SwaggerResponse(200, "Login realizado — token JWT retornado")]
        [SwaggerResponse(401, "Credenciais inválidas")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}