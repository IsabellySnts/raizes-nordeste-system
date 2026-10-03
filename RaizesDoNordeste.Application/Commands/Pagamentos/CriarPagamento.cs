using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Pagamentos;

public sealed record CriarPagamentoCommand : IRequest<ResultViewModel<CriarPagamentoResponse>>
{
    public long IdPedido { get; init; }
    public TipoPagamento TipoPagamento { get; init; }
}
