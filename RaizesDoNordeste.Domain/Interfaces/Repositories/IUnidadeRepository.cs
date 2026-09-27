using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IUnidadeRepository
{
    Task<Unidade> CriarAsync(Unidade unidade);
    Task<Unidade?> ObterPorIdAsync(long id);
    Task<Unidade?> ObterPorIdComDetalhesAsync(long id);
    Task<IEnumerable<Unidade>> ObterTodasAsync();
    Task AtualizarAsync(Unidade unidade);
    Task RemoverAsync(Unidade unidade);
    Task<bool> PossuiFuncionariosVinculadosAsync(long unidadeId);
    Task<bool> PossuiPedidosVinculadosAsync(long unidadeId);
    Task<bool> NomeExisteAsync(string nome, long? ignorarId = null);
}
