using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Produtos.ObterTodosProdutos;

public class ObterTodosProdutosHandler(IProdutoRepository _repository) : IRequestHandler<ObterTodosProdutosQuery, ResultViewModel<IEnumerable<ProdutoQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<ProdutoQueryResponse>>> Handle(ObterTodosProdutosQuery query, CancellationToken cancellationToken)
    {
        var produtos = await _repository.ObterTodosComCategoriaAsync();

        var response = produtos.Select(p => new ProdutoQueryResponse
        {
            Id = p.Id,
            Nome = p.Nome,
            IdCategoria = p.IdCategoria,
            NomeCategoria = p.Categoria?.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            FlagSazonal = p.FlagSazonal,
            DataInicioDisponibilidade = p.DataInicioDisponibilidade,
            DataFimDisponibilidade = p.DataFimDisponibilidade,
            Foto = p.Foto
        });

        return ResultViewModel<IEnumerable<ProdutoQueryResponse>>.Success(response);
    }
}
