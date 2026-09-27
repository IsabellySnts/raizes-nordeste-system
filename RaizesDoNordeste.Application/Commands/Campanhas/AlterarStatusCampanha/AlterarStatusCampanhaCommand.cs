using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Campanhas.AlterarStatusCampanha;

public sealed record AlterarStatusCampanhaCommand : IRequest<ResultViewModel<bool>>
{
    public long Id { get; init; }
    public StatusCampanha NovoStatus { get; init; }
}
