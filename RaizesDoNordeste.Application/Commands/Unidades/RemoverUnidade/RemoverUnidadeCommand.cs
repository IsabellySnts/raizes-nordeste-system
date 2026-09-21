using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Unidades.RemoverUnidade;

public sealed record RemoverUnidadeCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
}
