using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface ICategoriaRepository
{
    Task<Categoria> CriarAsync(Categoria categoria);
    Task<Categoria?> ObterPorIdAsync(long id);
    Task<IEnumerable<Categoria>> ObterTodasAsync();
}
