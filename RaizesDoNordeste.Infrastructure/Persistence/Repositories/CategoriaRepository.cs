using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class CategoriaRepository(RaizesDoNordesteDbContext dbContext) : ICategoriaRepository
{
    private readonly RaizesDoNordesteDbContext _dbContext = dbContext;

    public async Task<Categoria> CriarAsync(Categoria categoria)
    {
        _dbContext.Categorias.Add(categoria);
        _dbContext.SaveChanges();

        return categoria;
    }

    public async Task<Categoria?> ObterPorIdAsync(long id)
    {
        return await _dbContext.Categorias.FindAsync(id);
    }

    public async Task<IEnumerable<Categoria>> ObterTodasAsync()
    {
        return await _dbContext.Categorias.ToListAsync();
    }
}
