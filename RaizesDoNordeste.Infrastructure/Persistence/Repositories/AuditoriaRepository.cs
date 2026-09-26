using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public AuditoriaRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Auditoria> CriarAsync(Auditoria auditoria)
    {
        _context.Auditorias.Add(auditoria);
        await _context.SaveChangesAsync();
        return auditoria;
    }

    public async Task<IEnumerable<Auditoria>> ObterTodosAsync()
    {
        return await _context.Auditorias
            .Include(a => a.Funcionario)
            .OrderByDescending(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auditoria>> ObterPorFuncionarioAsync(long funcionarioId)
    {
        return await _context.Auditorias
            .Include(a => a.Funcionario)
            .Where(a => a.IdFuncionario == funcionarioId)
            .OrderByDescending(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auditoria>> ObterPorAcaoAsync(AcaoAuditoria acao)
    {
        return await _context.Auditorias
            .Include(a => a.Funcionario)
            .Where(a => a.Acao == acao)
            .OrderByDescending(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auditoria>> ObterPorEntidadeAsync(string tipoEntidade, long idEntidade)
    {
        return await _context.Auditorias
            .Include(a => a.Funcionario)
            .Where(a => a.TipoEntidadeAfetada == tipoEntidade && a.IdEntidadeAfetada == idEntidade)
            .OrderByDescending(a => a.DataHora)
            .ToListAsync();
    }
}