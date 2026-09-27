using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Pedidos.AtualizarStatusPedido;

public class AtualizarStatusPedidoHandler(IPedidoRepository _repository) : IRequestHandler<AtualizarStatusPedidoCommand, ResultViewModel<AtualizarStatusPedidoResponse>>
{
    public async Task<ResultViewModel<AtualizarStatusPedidoResponse>> Handle(
        AtualizarStatusPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = await _repository.ObterPorIdAsync(command.IdPedido);
        if (pedido == null)
            return ResultViewModel<AtualizarStatusPedidoResponse>.Error("Pedido não encontrado.");

        var statusAnterior = pedido.Status.ToString();

        var atualizou = pedido.AtualizarStatus(command.NovoStatus);
        if (!atualizou)
            return ResultViewModel<AtualizarStatusPedidoResponse>.Error(
                $"Transição de '{statusAnterior}' para '{command.NovoStatus}' não é permitida.");

        await _repository.AtualizarAsync(pedido);

        var response = new AtualizarStatusPedidoResponse
        {
            Id = pedido.Id,
            StatusAnterior = statusAnterior,
            StatusAtual = pedido.Status.ToString(),
            DataAtualizacao = pedido.DataAtualizacao
        };

        return ResultViewModel<AtualizarStatusPedidoResponse>.Success(response);
    }
}
