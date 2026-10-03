using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;

public class CriarPedidoHandler(
    IPedidoRepository _pedidoRepository,
    IUnidadeRepository _unidadeRepository,
    IPedidoService _pedidoService)
    : IRequestHandler<CriarPedidoCommand, ResultViewModel<CriarPedidoResponse>>
{
    public async Task<ResultViewModel<CriarPedidoResponse>> Handle(CriarPedidoCommand command, CancellationToken cancellationToken)
    {
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
            var validacao = await _pedidoService.ValidarItemAsync(itemCmd.IdProduto, command.IdUnidade, itemCmd.Quantidade);

            if (!validacao.IsSuccess)
                return ResultViewModel<CriarPedidoResponse>.Error(validacao.Message);

            var item = new ItemPedido(0, itemCmd.IdProduto, itemCmd.Quantidade, validacao.Data!.PrecoUnitario, itemCmd.Observacao);

            pedido.AdicionarItem(item);

            itensResponse.Add(new ItemPedidoResponse
            {
                IdProduto = itemCmd.IdProduto,
                NomeProduto = validacao.Data.NomeProduto,
                Quantidade = itemCmd.Quantidade,
                PrecoUnitario = validacao.Data.PrecoUnitario,
                Subtotal = validacao.Data.PrecoUnitario * itemCmd.Quantidade,
                Observacao = itemCmd.Observacao
            });

            await _pedidoService.ReduzirEstoqueAsync(itemCmd.IdProduto, command.IdUnidade, itemCmd.Quantidade);
        }

        decimal valorDesconto = 0;
        int pontosResgatados = 0;

        if (command.PontosParaResgatar is > 0 && command.IdCliente.HasValue)
        {
            var resgate = await _pedidoService.ResgatarPontosAsync(command.IdCliente.Value, command.PontosParaResgatar.Value, pedido.ValorTotal);
            if (!resgate.IsSuccess)
                return ResultViewModel<CriarPedidoResponse>.Error(resgate.Message);

            pontosResgatados = resgate.Data!.PontosResgatados;
            valorDesconto = resgate.Data.ValorDesconto;
            pedido.AplicarDesconto(valorDesconto);
        }

        pedido.AtualizarStatus(StatusPedido.AguardandoPagamento);
        var pedidoCriado = await _pedidoRepository.CriarAsync(pedido);

        for (int i = 0; i < itensResponse.Count; i++)
            itensResponse[i].Id = pedidoCriado.Itens.ElementAt(i).Id;

        var response = new CriarPedidoResponse
        {
            Id = pedidoCriado.Id,
            IdCliente = pedidoCriado.IdCliente,
            IdUnidade = pedidoCriado.IdUnidade,
            NomeUnidade = unidade.Nome,
            CanalOrigem = pedidoCriado.CanalOrigem.ToString(),
            Status = pedidoCriado.Status.ToString(),
            ValorTotal = pedidoCriado.ValorTotal,
            ValorDesconto = valorDesconto,
            PontosResgatados = pontosResgatados,
            DataCriacao = pedidoCriado.DataCriacao,
            Itens = itensResponse
        };

        return ResultViewModel<CriarPedidoResponse>.Success(response);
    }
}
