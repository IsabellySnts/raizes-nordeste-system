using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CancelarPedido;

public sealed record CancelarPedidoCommand : IRequest<ResultViewModel<CancelarPedidoResponse>>
{
    public long IdPedido { get; init; }
    public string Motivo { get; init; } = string.Empty;
    public long? IdFuncionario { get; init; } 
    public bool CanceladoPeloCliente { get; init; }
}
