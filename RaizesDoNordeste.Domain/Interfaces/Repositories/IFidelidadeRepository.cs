using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IFidelidadeRepository
{
    Task<Fidelidade> CriarAsync(Fidelidade fidelidade);
    Task<Fidelidade?> ObterPorIdAsync(long id);
    Task<Fidelidade?> ObterPorClienteIdAsync(long clienteId);
    Task<Fidelidade?> ObterPorClienteIdComMovimentacoesAsync(long clienteId);
    Task AtualizarAsync(Fidelidade fidelidade);
    Task<bool> ClienteJaPossuiFidelidadeAsync(long clienteId);
}
