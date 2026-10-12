using MediatR;
using RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Categorias.ObterTodasCategorias;

public class ObterTodasCategoriasHandler(ICategoriaRepository _repository) : IRequestHandler<ObterTodasCategoriasQuery, ResultViewModel<IEnumerable<CategoriaQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<CategoriaQueryResponse>>> Handle(ObterTodasCategoriasQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var categorias = await _repository.ObterTodasAsync();

            if (!categorias.Any())
                return ResultViewModel<IEnumerable<CategoriaQueryResponse>>.Error("Nenhuma categoria cadastrada até o momento.");

            var response = categorias.Select(c => new CategoriaQueryResponse
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao
            });

            return ResultViewModel<IEnumerable<CategoriaQueryResponse>>.Success(response);
        }
        catch(Exception ex)
        {
            return ResultViewModel<IEnumerable<CategoriaQueryResponse>>.Error($"Ocorreu um erro ao obter todas categorias: {ex.Message}", ResultStatus.InternalServerError);

        }
    }
}
