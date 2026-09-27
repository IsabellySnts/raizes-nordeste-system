using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Unidades.CriarUnidade;

public sealed record CriarUnidadeCommand : IRequest<ResultViewModel<CriarUnidadeResponse>>
{
    public string Nome { get; init; } = string.Empty;
    public string Cidade { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string Pais { get; init; } = string.Empty;
    public string Logradouro { get; init; } = string.Empty;
    public string? Complemento { get; init; }
    public string DiasFuncionamento { get; init; } = string.Empty;
    public string HorarioFuncionamento { get; init; } = string.Empty;
    public TipoCozinha TipoCozinha { get; init; }
}