using RaizesDoNordeste.Domain.Aggregates;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task<Pedido> CriarAsync(Pedido pedido);
    Task<Pedido?> ObterPorIdAsync(long id);
    Task<Pedido?> ObterPorIdComDetalhesAsync(long id);
    Task<IEnumerable<Pedido>> ObterPorUnidadeAsync(long unidadeId);
    Task<IEnumerable<Pedido>> ObterPorClienteAsync(long clienteId);
    Task<IEnumerable<Pedido>> ObterFilaCozinhaAsync(long unidadeId);
    Task AtualizarAsync(Pedido pedido);
}