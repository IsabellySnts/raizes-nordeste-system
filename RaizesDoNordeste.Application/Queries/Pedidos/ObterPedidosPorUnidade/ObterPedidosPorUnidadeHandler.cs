using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Pedidos.ObterPedidosPorUnidade;

public class ObterPedidosPorUnidadeHandler(IPedidoRepository _repository) : IRequestHandler<ObterPedidosPorUnidadeQuery, ResultViewModel<IEnumerable<PedidoResumoResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<PedidoResumoResponse>>> Handle(
        ObterPedidosPorUnidadeQuery query, CancellationToken cancellationToken)
    {
        var pedidos = await _repository.ObterPorUnidadeAsync(query.IdUnidade);

        var response = pedidos.Select(p => new PedidoResumoResponse
        {
            Id = p.Id,
            NomeCliente = p.Cliente?.NomeCompleto,
            CanalOrigem = p.CanalOrigem.ToString(),
            Status = p.Status.ToString(),
            ValorTotal = p.ValorTotal,
            TotalItens = p.Itens.Count,
            DataCriacao = p.DataCriacao
        });

        return ResultViewModel<IEnumerable<PedidoResumoResponse>>.Success(response);
    }
}
