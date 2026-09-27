using MediatR;
using RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Pedidos.ObterFilaCozinha;

public class ObterFilaCozinhaHandler(IPedidoRepository _repository) : IRequestHandler<ObterFilaCozinhaQuery, ResultViewModel<IEnumerable<FilaCozinhaResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<FilaCozinhaResponse>>> Handle(
        ObterFilaCozinhaQuery query, CancellationToken cancellationToken)
    {
        var pedidos = await _repository.ObterFilaCozinhaAsync(query.IdUnidade);

        var response = pedidos.Select(p => new FilaCozinhaResponse
        {
            IdPedido = p.Id,
            Status = p.Status.ToString(),
            DataCriacao = p.DataCriacao,
            Itens = p.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                IdProduto = i.IdProduto,
                NomeProduto = i.Produto?.Nome ?? "",
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Subtotal = i.PrecoUnitario * i.Quantidade,
                Observacao = i.Observacao
            }).ToList()
        });

        return ResultViewModel<IEnumerable<FilaCozinhaResponse>>.Success(response);
    }
}