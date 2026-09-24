using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class CardapioRepository : ICardapioRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public CardapioRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Cardapio> CriarAsync(Cardapio cardapio)
    {
        _context.Cardapios.Add(cardapio);
        await _context.SaveChangesAsync();
        return cardapio;
    }

    public async Task<Cardapio?> ObterPorIdAsync(long id)
    {
        return await _context.Cardapios.FindAsync(id);
    }

    public async Task<Cardapio?> ObterPorIdComDetalhesAsync(long id)
    {
        return await _context.Cardapios
            .Include(c => c.Produto)
                .ThenInclude(p => p!.Categoria)
            .Include(c => c.Unidade)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Cardapio>> ObterPorUnidadeAsync(long unidadeId)
    {
        return await _context.Cardapios
            .Include(c => c.Produto)
                .ThenInclude(p => p!.Categoria)
            .Where(c => c.IdUnidade == unidadeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cardapio>> ObterDisponivelPorUnidadeAsync(long unidadeId)
    {
        var agora = DateTime.UtcNow;

        return await _context.Cardapios
            .Include(c => c.Produto)
                .ThenInclude(p => p!.Categoria)
            .Where(c => c.IdUnidade == unidadeId
                && c.Disponivel
                && (!c.Produto!.FlagSazonal
                    || (c.Produto.DataInicioDisponibilidade <= agora
                        && c.Produto.DataFimDisponibilidade >= agora)))
            .ToListAsync();
    }

    public async Task AtualizarAsync(Cardapio cardapio)
    {
        _context.Cardapios.Update(cardapio);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Cardapio cardapio)
    {
        _context.Cardapios.Remove(cardapio);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ProdutoJaVinculadoAsync(long produtoId, long unidadeId, long? ignorarId = null)
    {
        return await _context.Cardapios
            .AnyAsync(c => c.IdProduto == produtoId
                && c.IdUnidade == unidadeId
                && (!ignorarId.HasValue || c.Id != ignorarId.Value));
    }
}
