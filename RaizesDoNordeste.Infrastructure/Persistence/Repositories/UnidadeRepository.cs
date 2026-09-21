using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class UnidadeRepository : IUnidadeRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public UnidadeRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Unidade> CriarAsync(Unidade unidade)
    {
        _context.Unidades.Add(unidade);
        await _context.SaveChangesAsync();
        return unidade;
    }

    public async Task<Unidade?> ObterPorIdAsync(long id)
    {
        return await _context.Unidades.FindAsync(id);
    }

    public async Task<Unidade?> ObterPorIdComDetalhesAsync(long id)
    {
        return await _context.Unidades
            .Include(u => u.Funcionarios)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<Unidade>> ObterTodasAsync()
    {
        return await _context.Unidades.ToListAsync();
    }

    public async Task AtualizarAsync(Unidade unidade)
    {
        _context.Unidades.Update(unidade);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Unidade unidade)
    {
        _context.Unidades.Remove(unidade);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> PossuiFuncionariosVinculadosAsync(long unidadeId)
    {
        return await _context.Funcionario.AnyAsync(f => f.IdUnidade == unidadeId);
    }

    public async Task<bool> PossuiPedidosVinculadosAsync(long unidadeId)
    {
        return await _context.Pedidos.AnyAsync(p => p.IdUnidade == unidadeId);
    }

    public async Task<bool> NomeExisteAsync(string nome, long? ignorarId = null)
    {
        return await _context.Unidades
            .AnyAsync(u => u.Nome == nome && (!ignorarId.HasValue || u.Id != ignorarId.Value));
    }
}
