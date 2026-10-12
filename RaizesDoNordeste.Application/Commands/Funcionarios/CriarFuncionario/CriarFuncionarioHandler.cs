using MediatR;
using RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.CriarFuncionario;

public class CriarFuncionarioHandler(
    IFuncionarioRepository _funcionarioRepository,
    IUsuarioRepository _usuarioRepository,
    IUnidadeRepository _unidadeRepository)
    : IRequestHandler<CriarFuncionarioCommand, ResultViewModel<CriarFuncionarioResponse>>
{
    public async Task<ResultViewModel<CriarFuncionarioResponse>> Handle(CriarFuncionarioCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var cpfExiste = await _funcionarioRepository.CpfExisteAsync(command.Cpf);

            if (cpfExiste)
                return ResultViewModel<CriarFuncionarioResponse>.Error("Este CPF já está cadastrado.");

            var emailExiste = await _usuarioRepository.EmailExisteAsync(command.Email);
            if (emailExiste)
                return ResultViewModel<CriarFuncionarioResponse>.Error("Este email já está cadastrado.");

            string? nomeUnidade = null;
            if (command.IdUnidade.HasValue)
            {
                var unidade = await _unidadeRepository.ObterPorIdAsync(command.IdUnidade.Value);
                if (unidade == null)
                    return ResultViewModel<CriarFuncionarioResponse>.Error("Unidade não encontrada.");

                nomeUnidade = unidade.Nome;
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(command.Senha);
            var usuario = new Usuario(command.Email, senhaHash, MapearPerfil(command.Cargo));
            var usuarioCriado = await _usuarioRepository.CriarAsync(usuario);

            var funcionario = new Funcionario(
                command.Nome,
                command.Cpf,
                command.Email,
                command.Telefone,
                command.IdUnidade,
                command.Cargo,
                usuarioCriado.Id
            );

            var funcionarioCriado = await _funcionarioRepository.CriarAsync(funcionario);

            var response = new CriarFuncionarioResponse
            {
                Id = funcionarioCriado.Id,
                Nome = funcionarioCriado.Nome,
                Email = funcionarioCriado.Email,
                Telefone = funcionarioCriado.Telefone,
                IdUnidade = funcionarioCriado.IdUnidade,
                NomeUnidade = nomeUnidade,
                Cargo = funcionarioCriado.Cargo.ToString(),
                Ativo = funcionarioCriado.Ativo
            };

            return ResultViewModel<CriarFuncionarioResponse>.Success(response);
        }
        catch(Exception ex)
        {
            return ResultViewModel<CriarFuncionarioResponse>.Error($"Ocorreu um erro ao criar funcionário: {ex.Message}", ResultStatus.InternalServerError);
        }
    }

    private static PerfilUsuario MapearPerfil(CargoFuncionario cargo)
    {
        return cargo switch
        {
            CargoFuncionario.Gerente => PerfilUsuario.Gerente,
            CargoFuncionario.GestorMatriz => PerfilUsuario.Admin,
            CargoFuncionario.Cozinheiro => PerfilUsuario.Cozinheiro,
            _ => PerfilUsuario.Atendente
        };
    }
}
