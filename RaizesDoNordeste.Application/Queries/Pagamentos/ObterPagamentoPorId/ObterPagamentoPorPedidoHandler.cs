using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Pagamentos.ObterPagamentoPorId;

public class ObterPagamentoPorPedidoHandler(IPagamentoRepository _repository) : IRequestHandler<ObterPagamentoPorPedidoQuery, ResultViewModel<PagamentoResponse>>
{
    public async Task<ResultViewModel<PagamentoResponse>> Handle(ObterPagamentoPorPedidoQuery query, CancellationToken cancellationToken)
    {
        var pagamento = await _repository.ObterPorPedidoIdAsync(query.IdPedido);
        if (pagamento == null)
            return ResultViewModel<PagamentoResponse>.Error("Pagamento não encontrado para este pedido.");

        var response = new PagamentoResponse
        {
            Id = pagamento.Id,
            IdPedido = pagamento.IdPedido,
            Valor = pagamento.Valor,
            TipoPagamento = pagamento.TipoPagamento.ToString(),
            Status = pagamento.Status.ToString(),
            DataSolicitacao = pagamento.DataSolicitacao,
            DataEfetivacao = pagamento.DataEfetivacao,
            CodigoTransacao = pagamento.CodigoTransacao,
            Tentativas = pagamento.Tentativas
        };

        return ResultViewModel<PagamentoResponse>.Success(response);
    }
}
