using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.AtivarFuncionario;

public class AtivarFuncionarioHandler(IFuncionarioRepository _repository) : IRequestHandler<AtivarFuncionarioCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(AtivarFuncionarioCommand command, CancellationToken cancellationToken)
    {
        var funcionario = await _repository.ObterPorIdAsync(command.Id);

        if (funcionario == null)
            return ResultViewModel<bool>.Error("Funcionário não encontrado.");

        if (funcionario.Ativo)
            return ResultViewModel<bool>.Error("Funcionário já está ativo.");

        funcionario.Ativar();
        await _repository.AtualizarAsync(funcionario);

        return ResultViewModel<bool>.Success(true, "Funcionário reativado com sucesso.");
    }
}
