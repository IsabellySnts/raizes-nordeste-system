using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.DesativarFuncionario;

public class DesativarFuncionarioHandler(IFuncionarioRepository _repository) : IRequestHandler<DesativarFuncionarioCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(
        DesativarFuncionarioCommand command, CancellationToken cancellationToken)
    {
        var funcionario = await _repository.ObterPorIdAsync(command.Id);

        if (funcionario == null)
            return ResultViewModel<bool>.Error("Funcionário não encontrado.");

        if (!funcionario.Ativo)
            return ResultViewModel<bool>.Error("Funcionário já está desativado.");

        funcionario.Desativar();
        await _repository.AtualizarAsync(funcionario);

        return ResultViewModel<bool>.Success(true, "Funcionário desativado com sucesso.");
    }
}