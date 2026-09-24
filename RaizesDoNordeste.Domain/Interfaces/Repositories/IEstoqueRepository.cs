using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IEstoqueRepository
{
    Task<Estoque> CriarAsync(Estoque estoque);
    Task<Estoque?> ObterPorIdAsync(long id);
    Task<Estoque?> ObterPorProdutoUnidadeAsync(long produtoId, long unidadeId);
    Task<IEnumerable<Estoque>> ObterPorUnidadeAsync(long unidadeId);
    Task AtualizarAsync(Estoque estoque);
    Task<bool> RegistroExisteAsync(long produtoId, long unidadeId, long? ignorarId = null);
}