namespace RaizesDoNordeste.Application.Queries.Clientes;

public class ClienteQueryResponse
{
    public long Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string CpfMascarado { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
    public int? SaldoPontos { get; set; }
    public string? NivelFidelidade { get; set; }
}
