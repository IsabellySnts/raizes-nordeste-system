using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RaizesDoNordeste.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string ComputeHash(string password)
    {
        using (var hash = SHA256.Create())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = hash.ComputeHash(passwordBytes);

            var builder = new StringBuilder();

            for (var i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public string GenerateToken(Usuario user)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não está configurado.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("username", user.Email),
        };

        var token = new JwtSecurityToken(issuer, audience, claims, null, DateTime.UtcNow.AddHours(2), credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
