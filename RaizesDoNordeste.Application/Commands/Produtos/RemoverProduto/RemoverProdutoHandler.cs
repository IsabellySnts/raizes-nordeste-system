using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Produtos.RemoverProduto;

public class RemoverProdutoHandler(IProdutoRepository _repository) : IRequestHandler<RemoverProdutoCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(RemoverProdutoCommand command, CancellationToken cancellationToken)
    {
        var produto = await _repository.ObterPorIdAsync(command.Id);

        if (produto == null)
            return ResultViewModel<bool>.Error("Produto não encontrado.");

        var possuiPedidos = await _repository.PossuiPedidosVinculadosAsync(command.Id);

        if (possuiPedidos)
            return ResultViewModel<bool>.Error("Não é possível remover um produto com pedidos vinculados.");

        await _repository.RemoverAsync(produto);

        return ResultViewModel<bool>.Success(true);
    }
}
