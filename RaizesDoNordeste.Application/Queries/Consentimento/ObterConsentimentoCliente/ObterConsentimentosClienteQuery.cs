using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.ConsentimentoLGPD.ObterConsentimentoCliente;

public sealed record ObterConsentimentosClienteQuery : IRequest<ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>>
{
    public long IdCliente { get; init; }
}
