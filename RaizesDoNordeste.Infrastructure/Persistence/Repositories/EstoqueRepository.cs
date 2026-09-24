using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class EstoqueRepository : IEstoqueRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public EstoqueRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Estoque> CriarAsync(Estoque estoque)
    {
        _context.Estoque.Add(estoque);
        await _context.SaveChangesAsync();
        return estoque;
    }

    public async Task<Estoque?> ObterPorIdAsync(long id)
    {
        return await _context.Estoque
            .Include(e => e.Produto)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Estoque?> ObterPorProdutoUnidadeAsync(long produtoId, long unidadeId)
    {
        return await _context.Estoque
            .Include(e => e.Produto)
            .FirstOrDefaultAsync(e => e.IdProduto == produtoId && e.IdUnidade == unidadeId);
    }

    public async Task<IEnumerable<Estoque>> ObterPorUnidadeAsync(long unidadeId)
    {
        return await _context.Estoque
            .Include(e => e.Produto)
                .ThenInclude(p => p!.Categoria)
            .Where(e => e.IdUnidade == unidadeId)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Estoque estoque)
    {
        _context.Estoque.Update(estoque);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RegistroExisteAsync(long produtoId, long unidadeId, long? ignorarId = null)
    {
        return await _context.Estoque
            .AnyAsync(e => e.IdProduto == produtoId
                && e.IdUnidade == unidadeId
                && (!ignorarId.HasValue || e.Id != ignorarId.Value));
    }
}
