using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class ConsentimentoLGPDRepository : IConsentimentoLGPDRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public ConsentimentoLGPDRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<ConsentimentoLGPD> CriarAsync(ConsentimentoLGPD consentimento)
    {
        _context.Consentimentos.Add(consentimento);
        await _context.SaveChangesAsync();
        return consentimento;
    }

    public async Task<ConsentimentoLGPD?> ObterPorIdAsync(long id)
    {
        return await _context.Consentimentos
            .Include(c => c.Cliente)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ConsentimentoLGPD>> ObterPorClienteAsync(long clienteId)
    {
        return await _context.Consentimentos
            .Where(c => c.IdCliente == clienteId)
            .OrderByDescending(c => c.Data)
            .ToListAsync();
    }

    public async Task<ConsentimentoLGPD?> ObterConsentimentoAtivoAsync(long clienteId, TipoConsentimento permissao)
    {
        return await _context.Consentimentos
            .FirstOrDefaultAsync(c => c.IdCliente == clienteId
                && c.Permissao == permissao
                && c.Aceite);
    }

    public async Task AtualizarAsync(ConsentimentoLGPD consentimento)
    {
        _context.Consentimentos.Update(consentimento);
        await _context.SaveChangesAsync();
    }
}