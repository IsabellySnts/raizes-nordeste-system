using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Pagamentos;

public class CriarPagamentoHandler(IPagamentoService _pagamentoService, IPedidoRepository _pedidoRepository) : IRequestHandler<CriarPagamentoCommand, ResultViewModel<CriarPagamentoResponse>>
{
    public async Task<ResultViewModel<CriarPagamentoResponse>> Handle(CriarPagamentoCommand command, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.IdPedido);

        if (pedido == null)
            return ResultViewModel<CriarPagamentoResponse>.Error("Pedido não encontrado.");

        var resultado = await _pagamentoService.CriarPagamentoAsync(
            command.IdPedido,
            pedido.ValorTotal,
            command.TipoPagamento
        );

        if (!resultado.IsSuccess)
            return ResultViewModel<CriarPagamentoResponse>.Error(resultado.Message);

        var response = new CriarPagamentoResponse
        {
            IdPagamento = resultado.Data!.IdPagamento,
            ClientSecret = resultado.Data.ClientSecret,
            StripePaymentIntentId = resultado.Data.StripePaymentIntentId,
            Status = StatusPagamento.Pendente
        };

        return ResultViewModel<CriarPagamentoResponse>.Success(response);
    }
}