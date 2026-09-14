using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Interfaces.Services;

public interface IAuthService
{
    string ComputeHash(string senha);
    string GenerateToken(Usuario usuario);
}
