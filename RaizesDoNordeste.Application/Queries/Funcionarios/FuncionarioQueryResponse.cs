namespace RaizesDoNordeste.Application.Queries.Funcionarios;

public class FuncionarioQueryResponse
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CpfMascarado { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public long? IdUnidade { get; set; }
    public string? NomeUnidade { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
