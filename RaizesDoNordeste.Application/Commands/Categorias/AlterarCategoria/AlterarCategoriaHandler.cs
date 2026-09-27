using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Categorias.AlterarCategoria;

public class AlterarCategoriaHandler(ICategoriaRepository _categoriaRepository) : IRequestHandler<AlterarCategoriaCommand, ResultViewModel<AlterarCategoriaResponse>>
{
    public async Task<ResultViewModel<AlterarCategoriaResponse>> Handle(AlterarCategoriaCommand command, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.IdCategoria);

        if (categoria == null)
            return ResultViewModel<AlterarCategoriaResponse>.Error("Categoria não encontrada.");

        categoria.Atualizar(command.Nome, command.Descricao);

        await _categoriaRepository.AtualizarAsync(categoria);

        var response = new AlterarCategoriaResponse
        {
            IdCategoria = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao
        };

        return ResultViewModel<AlterarCategoriaResponse>.Success(response);
    }
}
