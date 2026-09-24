using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Cardapios.DesvincularProdutoUnidade;

public sealed record DesvincularProdutoCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}
