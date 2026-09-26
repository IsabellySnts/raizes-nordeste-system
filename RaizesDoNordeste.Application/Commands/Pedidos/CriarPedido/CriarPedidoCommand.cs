using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;

public sealed record CriarPedidoCommand : IRequest<ResultViewModel<CriarPedidoResponse>>
{
    public long? IdCliente { get; init; }
    public long IdUnidade { get; init; }
    public long? IdFuncionario { get; init; }
    public CanalOrigem CanalOrigem { get; init; }
    public List<ItemPedidoCommand> Itens { get; init; } = new();
}

public sealed record ItemPedidoCommand
{
    public long IdProduto { get; init; }
    public int Quantidade { get; init; }
    public string? Observacao { get; init; }
}