using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Email { get; private set; }
    public string Senha { get; private set; }
    public PerfilUsuario Perfil { get; set; }

    protected Usuario() { }

    public Usuario(string email, string senha, PerfilUsuario perfil)
    {
        Email = email;
        Senha = senha;
        Perfil = perfil;
    }

    public void AtualizarSenha(string senha)
    {
        Senha = senha;
    }

    public void AtualizarEmail(string email)
    {
        Email = email;
    }

}
