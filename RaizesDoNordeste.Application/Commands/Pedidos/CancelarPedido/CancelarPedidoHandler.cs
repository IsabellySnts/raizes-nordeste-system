using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CancelarPedido;

public class CancelarPedidoHandler(
    IPedidoRepository _pedidoRepository,
    IEstoqueRepository _estoqueRepository)
    : IRequestHandler<CancelarPedidoCommand, ResultViewModel<CancelarPedidoResponse>>
{
    public async Task<ResultViewModel<CancelarPedidoResponse>> Handle(
        CancelarPedidoCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelarPedidoCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CancelarPedidoResponse>.Error(erros);
        }

        var pedido = await _pedidoRepository.ObterPorIdComDetalhesAsync(command.IdPedido);
        if (pedido == null)
            return ResultViewModel<CancelarPedidoResponse>.Error("Pedido não encontrado.");

        var statusAnterior = pedido.Status.ToString();

        if (command.CanceladoPeloCliente && !pedido.PodeCancelarPeloCliente())
            return ResultViewModel<CancelarPedidoResponse>.Error(
                "O pedido já está em preparo ou em etapa posterior. Apenas o gerente pode cancelar.");

        var cancelou = pedido.AtualizarStatus(StatusPedido.Cancelado);
        if (!cancelou)
            return ResultViewModel<CancelarPedidoResponse>.Error(
                $"Não é possível cancelar um pedido com status '{statusAnterior}'.");

        if (statusAnterior == StatusPedido.Pago.ToString()
            || statusAnterior == StatusPedido.EmPreparo.ToString()
            || statusAnterior == StatusPedido.Pronto.ToString())
        {
            foreach (var item in pedido.Itens)
            {
                var estoque = await _estoqueRepository.ObterPorProdutoUnidadeAsync(
                    item.IdProduto, pedido.IdUnidade);

                if (estoque != null)
                {
                    estoque.Repor(item.Quantidade);
                    await _estoqueRepository.AtualizarAsync(estoque);
                }
            }
        }

        await _pedidoRepository.AtualizarAsync(pedido);

        var response = new CancelarPedidoResponse
        {
            Id = pedido.Id,
            StatusAnterior = statusAnterior,
            StatusAtual = pedido.Status.ToString(),
            Motivo = command.Motivo,
            CanceladoPor = command.CanceladoPeloCliente ? "Cliente" : "Gerente",
            DataCancelamento = pedido.DataAtualizacao
        };

        return ResultViewModel<CancelarPedidoResponse>.Success(response);
    }
}
