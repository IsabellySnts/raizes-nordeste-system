using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Cardapios.DesvincularProdutoUnidade;

public class DesvincularProdutoHandler(ICardapioRepository _repository)
    : IRequestHandler<DesvincularProdutoCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(
        DesvincularProdutoCommand command, CancellationToken cancellationToken)
    {
        var cardapio = await _repository.ObterPorIdAsync(command.Id);

        if (cardapio == null)
            return ResultViewModel<bool>.Error("Item do cardápio não encontrado.");

        await _repository.RemoverAsync(cardapio);

        return ResultViewModel<bool>.Success(true, "Produto desvinculado da unidade com sucesso.");
    }
}
