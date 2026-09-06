using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Aggregates;

public class Cliente : BaseEntity
{
    public string NomeCompleto { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty; 
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public long IdUsuario { get; private set; }
    public Usuario? Usuario { get; private set; }
    public Fidelidade? Fidelidade { get; private set; }
    public ICollection<ConsentimentoLGPD> Consentimentos { get; private set; } = new List<ConsentimentoLGPD>();
    public ICollection<Pedido> Pedidos { get; private set; } = new List<Pedido>();

    protected Cliente() { }

    public Cliente(string nomeCompleto, string cpf, string telefone, string email, DateTime dataNascimento, long idUsuario)
    {
        NomeCompleto = nomeCompleto;
        Cpf = cpf;
        Telefone = telefone;
        Email = email;
        DataNascimento = dataNascimento;
        DataCadastro = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
        IdUsuario = idUsuario;
    }

    public void RegistrarConsentimento(ConsentimentoLGPD consentimento)
    {
        Consentimentos.Add(consentimento);
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Anonimizar()
    {
        NomeCompleto = "ANONIMIZADO";
        Cpf = "ANONIMIZADO";
        Telefone = "ANONIMIZADO";
        Email = "ANONIMIZADO";
        DataAtualizacao = DateTime.UtcNow;
    }
}

