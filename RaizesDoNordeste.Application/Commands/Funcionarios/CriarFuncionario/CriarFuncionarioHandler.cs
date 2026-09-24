using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.CriarFuncionario;

public class CriarFuncionarioHandler(
    IFuncionarioRepository _funcionarioRepository,
    IUsuarioRepository _usuarioRepository,
    IUnidadeRepository _unidadeRepository)
    : IRequestHandler<CriarFuncionarioCommand, ResultViewModel<CriarFuncionarioResponse>>
{
    public async Task<ResultViewModel<CriarFuncionarioResponse>> Handle(
        CriarFuncionarioCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarFuncionarioCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarFuncionarioResponse>.Error(erros);
        }

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
        var usuario = new Usuario(command.Email, senhaHash);
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
}
