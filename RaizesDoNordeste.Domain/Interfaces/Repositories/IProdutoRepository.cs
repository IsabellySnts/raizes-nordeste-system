using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IProdutoRepository
{
    Task<Produto> CriarAsync(Produto produto);
    Task<Produto?> ObterPorIdAsync(long id);
    Task<Produto?> ObterPorIdComCategoriaAsync(long id);
    Task<IEnumerable<Produto>> ObterTodosComCategoriaAsync();
    Task AtualizarAsync(Produto produto);
    Task RemoverAsync(Produto produto);
    Task<bool> PossuiPedidosVinculadosAsync(long produtoId);
}
