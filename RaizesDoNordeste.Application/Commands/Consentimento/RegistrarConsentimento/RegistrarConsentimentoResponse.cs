namespace RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;

public class RegistrarConsentimentoResponse
{
    public long Id { get; set; }
    public long IdCliente { get; set; }
    public string Permissao { get; set; } = string.Empty;
    public bool Aceite { get; set; }
    public DateTime Data { get; set; }
}