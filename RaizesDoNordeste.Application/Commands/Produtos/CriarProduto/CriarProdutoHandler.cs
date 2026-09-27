using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Produtos.CriarProduto;

public class CriarProdutoHandler(IProdutoRepository _produtoRepository, ICategoriaRepository _categoriaRepository)  : IRequestHandler<CriarProdutoCommand, ResultViewModel<CriarProdutoResponse>>
{
    public async Task<ResultViewModel<CriarProdutoResponse>> Handle(CriarProdutoCommand command, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.IdCategoria);
        if (categoria == null)
            return ResultViewModel<CriarProdutoResponse>.Error("Categoria não encontrada.");

        var produto = new Produto(
            command.Nome,
            command.IdCategoria,
            command.Descricao,
            command.Preco,
            command.FlagSazonal,
            command.DataInicioDisponibilidade,
            command.DataFimDisponibilidade,
            command.Foto
        );

        var produtoCriado = await _produtoRepository.CriarAsync(produto);

        var response = new CriarProdutoResponse
        {
            Id = produtoCriado.Id,
            Nome = produtoCriado.Nome,
            IdCategoria = produtoCriado.IdCategoria,
            Descricao = produtoCriado.Descricao,
            Preco = produtoCriado.Preco,
            FlagSazonal = produtoCriado.FlagSazonal,
            DataInicioDisponibilidade = produtoCriado.DataInicioDisponibilidade,
            DataFimDisponibilidade = produtoCriado.DataFimDisponibilidade,
            Foto = produtoCriado.Foto
        };

        return ResultViewModel<CriarProdutoResponse>.Success(response);
    }
}
