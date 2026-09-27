namespace RaizesDoNordeste.Application.Commands.Pedidos.AtualizarStatusPedido;

public class AtualizarStatusPedidoResponse
{
    public long Id { get; set; }
    public string StatusAnterior { get; set; } = string.Empty;
    public string StatusAtual { get; set; } = string.Empty;
    public DateTime DataAtualizacao { get; set; }
}