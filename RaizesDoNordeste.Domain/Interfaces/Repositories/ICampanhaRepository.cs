using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface ICampanhaRepository
{
    Task<Campanha> CriarAsync(Campanha campanha);
    Task<Campanha?> ObterPorIdAsync(long id);
    Task<IEnumerable<Campanha>> ObterTodasAsync();
    Task<IEnumerable<Campanha>> ObterAtivasAsync();
    Task AtualizarAsync(Campanha campanha);
    Task<bool> NomeExisteAsync(string nome, long? ignorarId = null);
}