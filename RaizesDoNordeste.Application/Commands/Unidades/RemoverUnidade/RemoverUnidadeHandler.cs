using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Unidades.RemoverUnidade;

public class RemoverUnidadeHandler(IUnidadeRepository _repository) : IRequestHandler<RemoverUnidadeCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(RemoverUnidadeCommand command, CancellationToken cancellationToken)
    {
        var unidade = await _repository.ObterPorIdAsync(command.Id);

        if (unidade == null)
            return ResultViewModel<bool>.Error("Unidade não encontrada.");

        var possuiFuncionarios = await _repository.PossuiFuncionariosVinculadosAsync(command.Id);
        if (possuiFuncionarios)
            return ResultViewModel<bool>.Error("Não é possível remover uma unidade com funcionários vinculados.");

        var possuiPedidos = await _repository.PossuiPedidosVinculadosAsync(command.Id);
        if (possuiPedidos)
            return ResultViewModel<bool>.Error("Não é possível remover uma unidade com pedidos vinculados.");

        await _repository.RemoverAsync(unidade);

        return ResultViewModel<bool>.Success(true);
    }
}
