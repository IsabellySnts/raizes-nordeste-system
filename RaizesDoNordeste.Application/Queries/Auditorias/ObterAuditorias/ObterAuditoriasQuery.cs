using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Queries.Auditorias.ObterAuditorias;

public sealed record ObterAuditoriasQuery : IRequest<ResultViewModel<IEnumerable<AuditoriaQueryResponse>>>
{
    public long? IdFuncionario { get; init; }
    public AcaoAuditoria? Acao { get; init; }
    public string? TipoEntidade { get; init; }
    public long? IdEntidade { get; init; }
}
