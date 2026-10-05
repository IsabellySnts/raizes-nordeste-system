using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Pagamentos.ObterPagamentoPorId;

public sealed record ObterPagamentoPorPedidoQuery(long IdPedido) : IRequest<ResultViewModel<PagamentoResponse>>;