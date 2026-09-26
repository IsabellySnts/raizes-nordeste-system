using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface ICardapioRepository
{
    Task<Cardapio> CriarAsync(Cardapio cardapio);
    Task<Cardapio?> ObterPorIdAsync(long id);
    Task<Cardapio?> ObterPorIdComDetalhesAsync(long id);
    Task<IEnumerable<Cardapio>> ObterPorUnidadeAsync(long unidadeId);
    Task<IEnumerable<Cardapio>> ObterDisponivelPorUnidadeAsync(long unidadeId);
    Task AtualizarAsync(Cardapio cardapio);
    Task RemoverAsync(Cardapio cardapio);
    Task<bool> ProdutoJaVinculadoAsync(long produtoId, long unidadeId, long? ignorarId = null);
    Task<Cardapio?> ObterPorProdutoUnidadeAsync(long produtoId, long unidadeId);

}
