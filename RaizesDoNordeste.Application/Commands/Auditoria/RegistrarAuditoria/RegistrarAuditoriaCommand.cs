using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Auditoria.RegistrarAuditoria;

public sealed record RegistrarAuditoriaCommand : IRequest<ResultViewModel<RegistrarAuditoriaResponse>>
{
    public long IdFuncionario { get; init; }
    public AcaoAuditoria Acao { get; init; }
    public string TipoEntidadeAfetada { get; init; } = string.Empty;
    public long IdEntidadeAfetada { get; init; }
    public string? Detalhes { get; init; }
}
