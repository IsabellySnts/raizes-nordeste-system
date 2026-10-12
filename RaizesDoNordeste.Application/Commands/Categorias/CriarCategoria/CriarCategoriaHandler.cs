using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;

public class CriarCategoriaHandler(ICategoriaRepository _repository) : IRequestHandler<CriarCategoriaCommand, ResultViewModel<CriarCategoriaResponse>>
{
    public async Task<ResultViewModel<CriarCategoriaResponse>> Handle(CriarCategoriaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await _repository.NomeExisteAsync(command.Nome))
                return ResultViewModel<CriarCategoriaResponse>.Error("Já existe uma categoria com este nome.");

            var categoria = new Categoria(command.Nome, command.Descricao);

            var categoriaCriada = await _repository.CriarAsync(categoria);

            var response = new CriarCategoriaResponse
            {
                IdCategoria = categoriaCriada.Id,
                Nome = categoriaCriada.Nome,
                Descricao = categoriaCriada.Descricao
            };

            return ResultViewModel<CriarCategoriaResponse>.Success(response, "Categoria criada com sucesso.");
        }
        catch(Exception ex)
        {
            return ResultViewModel<CriarCategoriaResponse>.Error($"Ocorreu um erro ao criar nova categoria: {ex.Message}", ResultStatus.InternalServerError);
        }
    }
}
