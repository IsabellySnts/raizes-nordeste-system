namespace RaizesDoNordeste.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Email { get; private set; }
    public string Senha { get; private set; }

    protected Usuario() { }

    public Usuario(string email, string senha)
    {
        Email = email;
        Senha = senha;
    }

    public void AtualizarSenha(string senha)
    {
        Senha = senha;
    }
}
