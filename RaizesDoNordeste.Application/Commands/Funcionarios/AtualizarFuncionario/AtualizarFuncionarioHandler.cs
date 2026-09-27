using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.AtualizarFuncionario;

public class AtualizarFuncionarioHandler(
    IFuncionarioRepository _funcionarioRepository,
    IUnidadeRepository _unidadeRepository)
    : IRequestHandler<AtualizarFuncionarioCommand, ResultViewModel<AtualizarFuncionarioResponse>>
{
    public async Task<ResultViewModel<AtualizarFuncionarioResponse>> Handle(
        AtualizarFuncionarioCommand command, CancellationToken cancellationToken)
    {
        var validator = new AtualizarFuncionarioCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AtualizarFuncionarioResponse>.Error(erros);
        }

        var funcionario = await _funcionarioRepository.ObterPorIdComDetalhesAsync(command.Id);
        if (funcionario == null)
            return ResultViewModel<AtualizarFuncionarioResponse>.Error("Funcionário não encontrado.");

        var emailExiste = await _funcionarioRepository.EmailExisteAsync(command.Email, command.Id);
        if (emailExiste)
            return ResultViewModel<AtualizarFuncionarioResponse>.Error("Este email já está em uso por outro funcionário.");

        string? nomeUnidade = null;
        if (command.IdUnidade.HasValue)
        {
            var unidade = await _unidadeRepository.ObterPorIdAsync(command.IdUnidade.Value);
            if (unidade == null)
                return ResultViewModel<AtualizarFuncionarioResponse>.Error("Unidade não encontrada.");
            nomeUnidade = unidade.Nome;
        }

        funcionario.Atualizar(command.Nome, command.Email, command.Telefone, command.IdUnidade, command.Cargo);

        await _funcionarioRepository.AtualizarAsync(funcionario);

        var response = new AtualizarFuncionarioResponse
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Email = funcionario.Email,
            Telefone = funcionario.Telefone,
            IdUnidade = funcionario.IdUnidade,
            NomeUnidade = nomeUnidade,
            Cargo = funcionario.Cargo.ToString(),
            Ativo = funcionario.Ativo
        };

        return ResultViewModel<AtualizarFuncionarioResponse>.Success(response);
    }
}
