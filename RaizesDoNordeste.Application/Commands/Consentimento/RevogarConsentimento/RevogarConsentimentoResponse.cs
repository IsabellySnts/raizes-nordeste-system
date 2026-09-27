namespace RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;

public class RevogarConsentimentoResponse
{
    public long Id { get; set; }
    public string Permissao { get; set; } = string.Empty;
    public DateTime DataRevogacao { get; set; }
}