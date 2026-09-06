using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class Unidade : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Cidade { get; private set; } = string.Empty;
    public string Estado { get; private set; } = string.Empty;
    public string Pais { get; private set; } = string.Empty;
    public string Logradouro { get; private set; } = string.Empty;
    public string? Complemento { get; private set; }
    public string DiasFuncionamento { get; private set; } = string.Empty;
    public string HorarioFuncionamento { get; private set; } = string.Empty;
    public TipoCozinha TipoCozinha { get; private set; }
    public ICollection<Funcionario> Funcionarios { get; private set; } = new List<Funcionario>();
    public ICollection<Cardapio> Cardapios { get; private set; } = new List<Cardapio>();
    public ICollection<Estoque> Estoques { get; private set; } = new List<Estoque>();
    public ICollection<Pedido> Pedidos { get; private set; } = new List<Pedido>();

    protected Unidade() { }

    public Unidade(string nome, string cidade, string estado, string pais,string logradouro, string? complemento, string diasFuncionamento, string horarioFuncionamento, TipoCozinha tipoCozinha)
    {
        Nome = nome;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
        Logradouro = logradouro;
        Complemento = complemento;
        DiasFuncionamento = diasFuncionamento;
        HorarioFuncionamento = horarioFuncionamento;
        TipoCozinha = tipoCozinha;
    }
}
