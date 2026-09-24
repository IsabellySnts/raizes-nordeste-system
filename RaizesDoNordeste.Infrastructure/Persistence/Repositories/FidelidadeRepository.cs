using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class FidelidadeRepository : IFidelidadeRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public FidelidadeRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Fidelidade> CriarAsync(Fidelidade fidelidade)
    {
        _context.Fidelidades.Add(fidelidade);
        await _context.SaveChangesAsync();
        return fidelidade;
    }

    public async Task<Fidelidade?> ObterPorIdAsync(long id)
    {
        return await _context.Fidelidades.FindAsync(id);
    }

    public async Task<Fidelidade?> ObterPorClienteIdAsync(long clienteId)
    {
        return await _context.Fidelidades
            .Include(f => f.Cliente)
            .FirstOrDefaultAsync(f => f.IdCliente == clienteId);
    }

    public async Task<Fidelidade?> ObterPorClienteIdComMovimentacoesAsync(long clienteId)
    {
        return await _context.Fidelidades
            .Include(f => f.Cliente)
            .Include(f => f.Movimentacoes.OrderByDescending(m => m.Data))
            .FirstOrDefaultAsync(f => f.IdCliente == clienteId);
    }

    public async Task AtualizarAsync(Fidelidade fidelidade)
    {
        _context.Fidelidades.Update(fidelidade);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ClienteJaPossuiFidelidadeAsync(long clienteId)
    {
        return await _context.Fidelidades.AnyAsync(f => f.IdCliente == clienteId);
    }
}
