using System.Security.Cryptography;
using System.Text;

namespace RaizesDoNordeste.Infrastructure.Security;

public static class CriptografiaService
{
    private static readonly string Chave = "SuaChaveSecreta32CaracteresAqui!";

    public static string Criptografar(string texto)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Chave);
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var bytes = Encoding.UTF8.GetBytes(texto);
        var encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

        var result = new byte[aes.IV.Length + encrypted.Length];
        aes.IV.CopyTo(result, 0);
        encrypted.CopyTo(result, aes.IV.Length);

        return Convert.ToBase64String(result);
    }

    public static string Descriptografar(string textoCriptografado)
    {
        var fullBytes = Convert.FromBase64String(textoCriptografado);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Chave);

        var iv = fullBytes[..16];
        var encrypted = fullBytes[16..];

        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor();
        var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

        return Encoding.UTF8.GetString(decrypted);
    }
}