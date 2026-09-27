namespace RaizesDoNordeste.Application.Commands.Clientes.CriarCliente;

public class CriarClienteResponse
{
    public long Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
}