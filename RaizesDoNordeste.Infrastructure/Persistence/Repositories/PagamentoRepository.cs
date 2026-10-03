using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public PagamentoRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }
    public async Task<Pagamento> CriarAsync(Pagamento pagamento)
    {
        await _context.Pagamentos.AddAsync(pagamento);
        await _context.SaveChangesAsync();
        return pagamento;
    }

    public async Task<Pagamento?> ObterPorIdAsync(long id)
    {
        return await _context.Pagamentos
            .Include(p => p.Pedido)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pagamento?> ObterPorPedidoIdAsync(long idPedido)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(p => p.IdPedido == idPedido);
    }

    public async Task AtualizarAsync(Pagamento pagamento)
    {
        _context.Pagamentos.Update(pagamento);
        await _context.SaveChangesAsync();
    }
}
