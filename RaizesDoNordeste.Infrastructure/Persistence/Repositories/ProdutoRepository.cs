using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public ProdutoRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Produto> CriarAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto?> ObterPorIdAsync(long id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    public async Task<Produto?> ObterPorIdComCategoriaAsync(long id)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Produto>> ObterTodosComCategoriaAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> PossuiPedidosVinculadosAsync(long produtoId)
    {
        return await _context.ItensPedido.AnyAsync(i => i.IdProduto == produtoId);
    }
}