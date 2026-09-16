using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Produtos.ObterProdutoPorId;

public class ObterProdutoPorIdHandler(IProdutoRepository _repository) : IRequestHandler<ObterProdutoPorIdQuery, ResultViewModel<ProdutoQueryResponse>>
{
    public async Task<ResultViewModel<ProdutoQueryResponse>> Handle(ObterProdutoPorIdQuery query, CancellationToken cancellationToken)
    {
        var produto = await _repository.ObterPorIdComCategoriaAsync(query.Id);

        if (produto == null)
            return ResultViewModel<ProdutoQueryResponse>.Error("Produto não encontrado.");

        var response = new ProdutoQueryResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            IdCategoria = produto.IdCategoria,
            NomeCategoria = produto.Categoria?.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            FlagSazonal = produto.FlagSazonal,
            DataInicioDisponibilidade = produto.DataInicioDisponibilidade,
            DataFimDisponibilidade = produto.DataFimDisponibilidade,
            Foto = produto.Foto
        };

        return ResultViewModel<ProdutoQueryResponse>.Success(response);
    }
}
