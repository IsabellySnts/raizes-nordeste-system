namespace RaizesDoNordeste.Application.Commands.Pedidos.CancelarPedido;

public class CancelarPedidoResponse
{
    public long Id { get; set; }
    public string StatusAnterior { get; set; } = string.Empty;
    public string StatusAtual { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public string CanceladoPor { get; set; } = string.Empty;
    public DateTime DataCancelamento { get; set; }
}