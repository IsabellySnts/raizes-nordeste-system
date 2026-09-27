using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Queries.Fidelidades.ObterExtratoFidelidade;

public sealed record ObterExtratoFidelidadeQuery : IRequest<ResultViewModel<ExtratoFidelidadeResponse>>
{
    public long IdCliente { get; init; }
}