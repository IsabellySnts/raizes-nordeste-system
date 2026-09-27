using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class CampanhaRepository : ICampanhaRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public CampanhaRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Campanha> CriarAsync(Campanha campanha)
    {
        _context.Campanhas.Add(campanha);
        await _context.SaveChangesAsync();
        return campanha;
    }

    public async Task<Campanha?> ObterPorIdAsync(long id)
    {
        return await _context.Campanhas.FindAsync(id);
    }

    public async Task<IEnumerable<Campanha>> ObterTodasAsync()
    {
        return await _context.Campanhas
            .OrderByDescending(c => c.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Campanha>> ObterAtivasAsync()
    {
        var agora = DateTime.UtcNow;

        return await _context.Campanhas
            .Where(c => c.Status == StatusCampanha.Ativa
                && c.DataInicio <= agora
                && c.DataFim >= agora)
            .OrderBy(c => c.DataFim)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Campanha campanha)
    {
        _context.Campanhas.Update(campanha);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> NomeExisteAsync(string nome, long? ignorarId = null)
    {
        return await _context.Campanhas
            .AnyAsync(c => c.Nome == nome && (!ignorarId.HasValue || c.Id != ignorarId.Value));
    }
}