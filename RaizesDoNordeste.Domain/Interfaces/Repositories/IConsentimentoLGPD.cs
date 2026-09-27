using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Interfaces.Repositories;

public interface IConsentimentoLGPDRepository
{
    Task<ConsentimentoLGPD> CriarAsync(ConsentimentoLGPD consentimento);
    Task<ConsentimentoLGPD?> ObterPorIdAsync(long id);
    Task<IEnumerable<ConsentimentoLGPD>> ObterPorClienteAsync(long clienteId);
    Task<ConsentimentoLGPD?> ObterConsentimentoAtivoAsync(long clienteId, TipoConsentimento permissao);
    Task AtualizarAsync(ConsentimentoLGPD consentimento);
}
