using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class Fidelidade : BaseEntity
{
    public long IdCliente { get; private set; }
    public int SaldoPontos { get; private set; }
    public NivelFidelidade Nivel { get; private set; }

    public Cliente? Cliente { get; private set; }
    public ICollection<MovimentacaoPontos> Movimentacoes { get; private set; } = new List<MovimentacaoPontos>();

    protected Fidelidade() { }

    public Fidelidade(long idCliente)
    {
        IdCliente = idCliente;
        SaldoPontos = 0;
        Nivel = NivelFidelidade.Bronze;
    }

    public void AcumularPontos(int pontos, long? idPedido)
    {
        SaldoPontos += pontos;
        Movimentacoes.Add(new MovimentacaoPontos(
            Id, idPedido, TipoMovimentacaoPontos.Acumulo, pontos));
        CalcularNivel();
    }

    public bool ResgatarPontos(int pontos, long? idPedido)
    {
        if (SaldoPontos < pontos) return false;
        SaldoPontos -= pontos;
        Movimentacoes.Add(new MovimentacaoPontos(
            Id, idPedido, TipoMovimentacaoPontos.Resgate, pontos));
        CalcularNivel();
        return true;
    }

    private void CalcularNivel()
    {
        Nivel = SaldoPontos switch
        {
            >= 1000 => NivelFidelidade.Ouro,
            >= 500 => NivelFidelidade.Prata,
            _ => NivelFidelidade.Bronze
        };
    }
}
