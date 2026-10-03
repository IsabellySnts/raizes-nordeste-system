using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IPagamentoRepository
{
    Task<Pagamento> CriarAsync(Pagamento pagamento);
    Task<Pagamento?> ObterPorIdAsync(long id);
    Task<Pagamento?> ObterPorPedidoIdAsync(long idPedido);
    Task AtualizarAsync(Pagamento pagamento);
}
