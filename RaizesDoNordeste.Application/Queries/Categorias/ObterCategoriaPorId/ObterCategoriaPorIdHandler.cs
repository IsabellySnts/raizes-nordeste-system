using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Categorias.ObterCategoriaPorId;

public class ObterCategoriaPorIdHandler(ICategoriaRepository _repository) : IRequestHandler<ObterCategoriaPorIdQuery, ResultViewModel<CategoriaQueryResponse>>
{
    public async Task<ResultViewModel<CategoriaQueryResponse>> Handle(ObterCategoriaPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var categoria = await _repository.ObterPorIdAsync(query.Id);

            if (categoria == null)
                return ResultViewModel<CategoriaQueryResponse>.Error("Categoria não foi encontrada.");

            var response = new CategoriaQueryResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao
            };

            return ResultViewModel<CategoriaQueryResponse>.Success(response);
        }
        catch(Exception ex)
        {
            return ResultViewModel<CategoriaQueryResponse>.Error($"Ocorreu um erro ao consultar categoria: {ex.Message}", ResultStatus.InternalServerError);
        }
    }
}
