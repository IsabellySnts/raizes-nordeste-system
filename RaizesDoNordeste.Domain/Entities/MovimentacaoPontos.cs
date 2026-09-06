using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class MovimentacaoPontos : BaseEntity
{
    public long IdFidelidade { get; private set; }
    public long? IdPedido { get; private set; }
    public TipoMovimentacaoPontos Tipo { get; private set; }
    public int Pontos { get; private set; }
    public DateTime Data { get; private set; }
    public Fidelidade? Fidelidade { get; private set; }
    public Pedido? Pedido { get; private set; }

    protected MovimentacaoPontos() { }

    public MovimentacaoPontos(long idFidelidade, long? idPedido, TipoMovimentacaoPontos tipo, int pontos)
    {
        IdFidelidade = idFidelidade;
        IdPedido = idPedido;
        Tipo = tipo;
        Pontos = pontos;
        Data = DateTime.UtcNow;
    }
}