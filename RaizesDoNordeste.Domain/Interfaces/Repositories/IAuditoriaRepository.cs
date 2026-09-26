using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IAuditoriaRepository
{
    Task<Auditoria> CriarAsync(Auditoria auditoria);
    Task<IEnumerable<Auditoria>> ObterTodosAsync();
    Task<IEnumerable<Auditoria>> ObterPorFuncionarioAsync(long funcionarioId);
    Task<IEnumerable<Auditoria>> ObterPorAcaoAsync(AcaoAuditoria acao);
    Task<IEnumerable<Auditoria>> ObterPorEntidadeAsync(string tipoEntidade, long idEntidade);
}
