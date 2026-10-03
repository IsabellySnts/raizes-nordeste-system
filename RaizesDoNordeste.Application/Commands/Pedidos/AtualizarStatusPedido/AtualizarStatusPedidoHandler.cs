using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Pedidos.AtualizarStatusPedido;

public class AtualizarStatusPedidoHandler(IPedidoRepository _repository, IFidelidadeRepository _fidelidadeRepository) : IRequestHandler<AtualizarStatusPedidoCommand, ResultViewModel<AtualizarStatusPedidoResponse>>
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

        int? pontosAcumulados = null;

        if (command.NovoStatus == StatusPedido.Entregue && pedido.IdCliente.HasValue)
        {
            var fidelidade = await _fidelidadeRepository.ObterPorClienteIdAsync(pedido.IdCliente.Value);
            if (fidelidade != null)
            {
                var pontos = (int)Math.Floor(pedido.ValorTotal);
                fidelidade.AcumularPontos(pontos, pedido.Id);
                await _fidelidadeRepository.AtualizarAsync(fidelidade);
                pontosAcumulados = pontos;
            }
        }

        var response = new AtualizarStatusPedidoResponse
        {
            Id = pedido.Id,
            StatusAnterior = statusAnterior,
            StatusAtual = pedido.Status.ToString(),
            DataAtualizacao = pedido.DataAtualizacao,
            PontosAcumulados = pontosAcumulados
        };

        return ResultViewModel<AtualizarStatusPedidoResponse>.Success(response);
    }
}
