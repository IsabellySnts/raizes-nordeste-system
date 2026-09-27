namespace RaizesDoNordeste.Application.Commands.Clientes.AtualizarCliente;

public class AtualizarClienteResponse
{
    public long Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public DateTime DataAtualizacao { get; set; }
}