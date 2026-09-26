using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public PedidoRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido> CriarAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<Pedido?> ObterPorIdAsync(long id)
    {
        return await _context.Pedidos.FindAsync(id);
    }

    public async Task<Pedido?> ObterPorIdComDetalhesAsync(long id)
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Unidade)
            .Include(p => p.Funcionario)
            .Include(p => p.Pagamento)
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pedido>> ObterPorUnidadeAsync(long unidadeId)
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Itens)
            .Where(p => p.IdUnidade == unidadeId)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pedido>> ObterPorClienteAsync(long clienteId)
    {
        return await _context.Pedidos
            .Include(p => p.Unidade)
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .Include(p => p.Pagamento)
            .Where(p => p.IdCliente == clienteId)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pedido>> ObterFilaCozinhaAsync(long unidadeId)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .Where(p => p.IdUnidade == unidadeId
                && (p.Status == StatusPedido.Pago || p.Status == StatusPedido.EmPreparo))
            .OrderBy(p => p.DataCriacao)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
    }
}
