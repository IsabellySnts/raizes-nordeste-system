using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Campanhas.CriarCampanha;

public sealed record CriarCampanhaCommand : IRequest<ResultViewModel<CriarCampanhaResponse>>
{
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public string? Criterios { get; init; }
    public DateTime DataInicio { get; init; }
    public DateTime DataFim { get; init; }
    public string? Beneficio { get; init; }
}