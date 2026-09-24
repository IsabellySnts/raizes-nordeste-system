using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IFuncionarioRepository
{
    Task<Funcionario> CriarAsync(Funcionario funcionario);
    Task<Funcionario?> ObterPorIdAsync(long id);
    Task<Funcionario?> ObterPorIdComDetalhesAsync(long id);
    Task<IEnumerable<Funcionario>> ObterTodosAsync();
    Task<IEnumerable<Funcionario>> ObterPorUnidadeAsync(long unidadeId);
    Task AtualizarAsync(Funcionario funcionario);
    Task<bool> CpfExisteAsync(string cpf, long? ignorarId = null);
    Task<bool> EmailExisteAsync(string email, long? ignorarId = null);
}