using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Campanhas.AtualizarCampanha;

public sealed record AtualizarCampanhaCommand : IRequest<ResultViewModel<AtualizarCampanhaResponse>>
{
    public long Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public string? Criterios { get; init; }
    public DateTime DataInicio { get; init; }
    public DateTime DataFim { get; init; }
    public string? Beneficio { get; init; }
}