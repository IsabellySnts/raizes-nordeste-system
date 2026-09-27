namespace RaizesDoNordeste.Application.Queries.ConsentimentoLGPD.ObterConsentimentoCliente;

public class ConsentimentoQueryResponse
{
    public long Id { get; set; }
    public string Permissao { get; set; } = string.Empty;
    public bool Aceite { get; set; }
    public DateTime Data { get; set; }
}