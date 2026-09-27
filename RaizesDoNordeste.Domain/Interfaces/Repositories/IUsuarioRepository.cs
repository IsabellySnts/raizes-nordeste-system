using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario> CriarAsync(Usuario usuario);
    Task<Usuario?> ObterPorIdAsync(long id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task AtualizarAsync(Usuario usuario);
    Task RemoverAsync(Usuario usuario);
    Task<bool> EmailExisteAsync(string email, long? ignorarId = null);
}
