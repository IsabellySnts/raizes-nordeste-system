using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class Campanha : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public string? Criterios { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public StatusCampanha Status { get; private set; }
    public string? Beneficio { get; private set; }

    protected Campanha() { }

    public Campanha(string nome, string? descricao, string? criterios,  DateTime dataInicio, DateTime dataFim, string? beneficio)
    {
        Nome = nome;
        Descricao = descricao;
        Criterios = criterios;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Status = StatusCampanha.Ativa;
        Beneficio = beneficio;
    }

    public bool EstaAtiva(DateTime dataAtual)
    {
        return Status == StatusCampanha.Ativa &&
               dataAtual >= DataInicio &&
               dataAtual <= DataFim;
    }

    public void Encerrar()
    {
        Status = StatusCampanha.Encerrada;
    }

    public void Desativar()
    {
        Status = StatusCampanha.Inativa;
    }
}
