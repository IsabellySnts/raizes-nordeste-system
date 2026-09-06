using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class Funcionario : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public long? IdUnidade { get; private set; } 
    public CargoFuncionario Cargo { get; private set; }
    public long IdUsuario { get; private set; }
    public bool Ativo { get; private set; } = true;
    public Usuario? Usuario { get; private set; }
    public Unidade? Unidade { get; private set; }

    protected Funcionario() { }

    public Funcionario(string nome, string cpf, string email, string telefone, long? idUnidade, CargoFuncionario cargo, long idUsuario)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        Telefone = telefone;
        IdUnidade = idUnidade;
        Cargo = cargo;
        IdUsuario = idUsuario;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Ativar()
    {
        Ativo = true;
    }
}
