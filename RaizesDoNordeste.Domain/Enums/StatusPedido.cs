namespace RaizesDoNordeste.Domain.Enums;

public enum StatusPedido
{
    Criado = 1,
    AguardandoPagamento = 2,
    Pago = 3,
    EmPreparo = 4,
    Pronto = 5,
    Entregue = 6,
    Cancelado = 7,
    Recusado = 8,
    ErroPagamento = 9
}
