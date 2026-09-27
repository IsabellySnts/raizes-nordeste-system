using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;

public class CriarPedidoHandler(
    IPedidoRepository _pedidoRepository,
    IUnidadeRepository _unidadeRepository,
    ICardapioRepository _cardapioRepository,
    IEstoqueRepository _estoqueRepository)
    : IRequestHandler<CriarPedidoCommand, ResultViewModel<CriarPedidoResponse>>
{
    public async Task<ResultViewModel<CriarPedidoResponse>> Handle(
        CriarPedidoCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarPedidoCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarPedidoResponse>.Error(erros);
        }

        var unidade = await _unidadeRepository.ObterPorIdAsync(command.IdUnidade);
        if (unidade == null)
            return ResultViewModel<CriarPedidoResponse>.Error("Unidade não encontrada.");

        var pedido = new Pedido(
            command.IdCliente,
            command.IdUnidade,
            command.IdFuncionario,
            command.CanalOrigem
        );

        var itensResponse = new List<ItemPedidoResponse>();

        foreach (var itemCmd in command.Itens)
        {
            var cardapio = await _cardapioRepository.ObterPorProdutoUnidadeAsync(itemCmd.IdProduto, command.IdUnidade);
            if (cardapio == null || !cardapio.Disponivel)
                return ResultViewModel<CriarPedidoResponse>.Error(
                    $"Produto {itemCmd.IdProduto} não está disponível no cardápio desta unidade.");

            var estoque = await _estoqueRepository.ObterPorProdutoUnidadeAsync(itemCmd.IdProduto, command.IdUnidade);
            if (estoque == null || !estoque.EstaDisponivel())
                return ResultViewModel<CriarPedidoResponse>.Error(
                    $"Produto {itemCmd.IdProduto} está com estoque indisponível.");

            if (estoque.Quantidade < itemCmd.Quantidade)
                return ResultViewModel<CriarPedidoResponse>.Error(
                    $"Estoque insuficiente para o produto {itemCmd.IdProduto}. Disponível: {estoque.Quantidade}.");

            var preco = cardapio.PrecoLocal ?? cardapio.Produto?.Preco ?? 0;

            var item = new ItemPedido(
                0, 
                itemCmd.IdProduto,
                itemCmd.Quantidade,
                preco,
                itemCmd.Observacao
            );

            pedido.AdicionarItem(item);

            itensResponse.Add(new ItemPedidoResponse
            {
                IdProduto = itemCmd.IdProduto,
                NomeProduto = cardapio.Produto?.Nome ?? "",
                Quantidade = itemCmd.Quantidade,
                PrecoUnitario = preco,
                Subtotal = preco * itemCmd.Quantidade,
                Observacao = itemCmd.Observacao
            });
        }

        pedido.AtualizarStatus(StatusPedido.AguardandoPagamento);

        var pedidoCriado = await _pedidoRepository.CriarAsync(pedido);

        for (int i = 0; i < itensResponse.Count; i++)
        {
            itensResponse[i].Id = pedidoCriado.Itens.ElementAt(i).Id;
        }

        var response = new CriarPedidoResponse
        {
            Id = pedidoCriado.Id,
            IdCliente = pedidoCriado.IdCliente,
            IdUnidade = pedidoCriado.IdUnidade,
            NomeUnidade = unidade.Nome,
            CanalOrigem = pedidoCriado.CanalOrigem.ToString(),
            Status = pedidoCriado.Status.ToString(),
            ValorTotal = pedidoCriado.ValorTotal,
            DataCriacao = pedidoCriado.DataCriacao,
            Itens = itensResponse
        };

        return ResultViewModel<CriarPedidoResponse>.Success(response);
    }
}
