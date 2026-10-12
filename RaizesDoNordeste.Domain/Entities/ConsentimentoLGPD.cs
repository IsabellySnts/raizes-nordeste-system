using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class ConsentimentoLGPD : BaseEntity
{
    public long IdCliente { get; private set; }
    public TipoConsentimento Permissao { get; private set; }
    public bool Aceite { get; private set; }
    public DateTime Data { get; private set; }
    public Cliente? Cliente { get; private set; }

    protected ConsentimentoLGPD() { }

    public ConsentimentoLGPD(long idCliente, TipoConsentimento permissao)
    {
        IdCliente = idCliente;
        Permissao = permissao;
        Aceite = true;
        Data = DateTime.Now;
    }

    public void Revogar()
    {
        Aceite = false;
        Data = DateTime.Now;
    }
}