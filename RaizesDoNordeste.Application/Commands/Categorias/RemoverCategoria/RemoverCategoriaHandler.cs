using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Categorias.RemoverCategoria;

public class RemoverCategoriaHandler(ICategoriaRepository _repository)
    : IRequestHandler<RemoverCategoriaCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(RemoverCategoriaCommand command, CancellationToken cancellationToken)
    {

        var categoria = await _repository.ObterPorIdAsync(command.Id);

        if (categoria == null)
            return ResultViewModel<bool>.Error("Categoria não foi encontrada.");

        var possuiProdutos = await _repository.PossuiProdutosVinculadosAsync(command.Id);

        if (possuiProdutos)
            return ResultViewModel<bool>.Error("Não é possível remover uma categoria com produtos vinculados.");

        await _repository.RemoverAsync(categoria);

        return ResultViewModel<bool>.Success(true);
    }
}