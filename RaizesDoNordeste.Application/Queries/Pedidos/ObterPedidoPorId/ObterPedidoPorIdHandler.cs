using MediatR;
using RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidoPorId;

public class ObterPedidoPorIdHandler(IPedidoRepository _repository)
    : IRequestHandler<ObterPedidoPorIdQuery, ResultViewModel<PedidoQueryResponse>>
{
    public async Task<ResultViewModel<PedidoQueryResponse>> Handle(
        ObterPedidoPorIdQuery query, CancellationToken cancellationToken)
    {
        var pedido = await _repository.ObterPorIdComDetalhesAsync(query.Id);

        if (pedido == null)
            return ResultViewModel<PedidoQueryResponse>.Error("Pedido não encontrado.");

        var response = new PedidoQueryResponse
        {
            Id = pedido.Id,
            IdCliente = pedido.IdCliente,
            NomeCliente = pedido.Cliente?.NomeCompleto,
            IdUnidade = pedido.IdUnidade,
            NomeUnidade = pedido.Unidade?.Nome ?? "",
            IdFuncionario = pedido.IdFuncionario,
            NomeFuncionario = pedido.Funcionario?.Nome,
            CanalOrigem = pedido.CanalOrigem.ToString(),
            Status = pedido.Status.ToString(),
            ValorTotal = pedido.ValorTotal,
            DataCriacao = pedido.DataCriacao,
            DataAtualizacao = pedido.DataAtualizacao,
            StatusPagamento = pedido.Pagamento?.Status.ToString(),
            Itens = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                IdProduto = i.IdProduto,
                NomeProduto = i.Produto?.Nome ?? "",
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Subtotal = i.PrecoUnitario * i.Quantidade,
                Observacao = i.Observacao
            }).ToList()
        };

        return ResultViewModel<PedidoQueryResponse>.Success(response);
    }
}