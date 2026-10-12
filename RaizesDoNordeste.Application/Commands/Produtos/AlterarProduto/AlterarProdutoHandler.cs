using MediatR;
using RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Produtos.AlterarProduto;

public class AlterarProdutoHandler(IProdutoRepository _produtoRepository, ICategoriaRepository _categoriaRepository) : IRequestHandler<AlterarProdutoCommand, ResultViewModel<AlterarProdutoResponse>>
{
    public async Task<ResultViewModel<AlterarProdutoResponse>> Handle(AlterarProdutoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(command.Id);

            if (produto == null)
                return ResultViewModel<AlterarProdutoResponse>.Error("Produto não encontrado.");

            var categoria = await _categoriaRepository.ObterPorIdAsync(command.IdCategoria);

            if (categoria == null)
                return ResultViewModel<AlterarProdutoResponse>.Error("Categoria não encontrada.");

            produto.Atualizar(
                command.Nome,
                command.IdCategoria,
                command.Descricao,
                command.Preco,
                command.FlagSazonal,
                command.DataInicioDisponibilidade,
                command.DataFimDisponibilidade,
                command.Foto
            );

            await _produtoRepository.AtualizarAsync(produto);

            var response = new AlterarProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                IdCategoria = produto.IdCategoria,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                FlagSazonal = produto.FlagSazonal,
                DataInicioDisponibilidade = produto.DataInicioDisponibilidade,
                DataFimDisponibilidade = produto.DataFimDisponibilidade,
                Foto = produto.Foto
            };

            return ResultViewModel<AlterarProdutoResponse>.Success(response);
        }
        catch(Exception ex)
        {
            return ResultViewModel<AlterarProdutoResponse>.Error($"Ocorreu um erro ao atualizar o produto: {ex.Message}", ResultStatus.InternalServerError);
        }
    }
}
