using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Cardapios.AtualizarCardapio;

public sealed record AtualizarCardapioCommand : IRequest<ResultViewModel<AtualizarCardapioResponse>>
{
    public long Id { get; init; }
    public bool Disponivel { get; init; }
    public decimal? PrecoLocal { get; init; }
    public string? VariacaoRegional { get; init; }
}
