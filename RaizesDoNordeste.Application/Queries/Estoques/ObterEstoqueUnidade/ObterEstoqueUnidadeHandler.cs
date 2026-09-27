using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Estoques.ObterEstoqueUnidade;

public class ObterEstoqueUnidadeHandler(IEstoqueRepository _estoqueRepository,IUnidadeRepository _unidadeRepository) : IRequestHandler<ObterEstoqueUnidadeQuery, ResultViewModel<IEnumerable<EstoqueQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<EstoqueQueryResponse>>> Handle(
        ObterEstoqueUnidadeQuery query, CancellationToken cancellationToken)
    {
        var unidade = await _unidadeRepository.ObterPorIdAsync(query.IdUnidade);
        if (unidade == null)
            return ResultViewModel<IEnumerable<EstoqueQueryResponse>>.Error("Unidade não encontrada.");

        var estoques = await _estoqueRepository.ObterPorUnidadeAsync(query.IdUnidade);

        var response = estoques.Select(e => new EstoqueQueryResponse
        {
            Id = e.Id,
            IdProduto = e.IdProduto,
            NomeProduto = e.Produto?.Nome ?? "",
            CategoriaProduto = e.Produto?.Categoria?.Nome,
            Quantidade = e.Quantidade,
            QuantidadeMinima = e.QuantidadeMinima,
            Disponivel = e.EstaDisponivel(),
            Status = ObterStatus(e),
            DataAtualizacao = e.DataAtualizacao
        });

        return ResultViewModel<IEnumerable<EstoqueQueryResponse>>.Success(response);
    }

    private static string ObterStatus(Domain.Entities.Estoque estoque)
    {
        if (estoque.Quantidade == 0)
            return "Esgotado";
        if (!estoque.EstaDisponivel())
            return "Abaixo do mínimo";
        return "Disponível";
    }
}