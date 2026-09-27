using RaizesDoNordeste.Domain.Aggregates;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IClienteRepository
{
    Task<Cliente> CriarAsync(Cliente cliente);
    Task<Cliente?> ObterPorIdAsync(long id);
    Task<Cliente?> ObterPorIdComUsuarioAsync(long id);
    Task<Cliente?> ObterPorCpfAsync(string cpf);
    Task<IEnumerable<Cliente>> ObterTodosAsync();
    Task AtualizarAsync(Cliente cliente);
    Task<bool> CpfExisteAsync(string cpf, long? ignorarId = null);
    Task<bool> EmailExisteAsync(string email, long? ignorarId = null);
}
