using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Estoques.CriarEstoque;

public class CriarEstoqueHandler(
    IEstoqueRepository _estoqueRepository,
    IProdutoRepository _produtoRepository,
    IUnidadeRepository _unidadeRepository)
    : IRequestHandler<CriarEstoqueCommand, ResultViewModel<CriarEstoqueResponse>>
{
    public async Task<ResultViewModel<CriarEstoqueResponse>> Handle(
        CriarEstoqueCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarEstoqueCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarEstoqueResponse>.Error(erros);
        }

        var produto = await _produtoRepository.ObterPorIdAsync(command.IdProduto);
        if (produto == null)
            return ResultViewModel<CriarEstoqueResponse>.Error("Produto não encontrado.");

        var unidade = await _unidadeRepository.ObterPorIdAsync(command.IdUnidade);
        if (unidade == null)
            return ResultViewModel<CriarEstoqueResponse>.Error("Unidade não encontrada.");

        var jaExiste = await _estoqueRepository.RegistroExisteAsync(command.IdProduto, command.IdUnidade);
        if (jaExiste)
            return ResultViewModel<CriarEstoqueResponse>.Error("Já existe registro de estoque para este produto nesta unidade.");

        var estoque = new Estoque(
            command.IdProduto,
            command.IdUnidade,
            command.Quantidade,
            command.QuantidadeMinima
        );

        var estoqueCriado = await _estoqueRepository.CriarAsync(estoque);

        var response = new CriarEstoqueResponse
        {
            Id = estoqueCriado.Id,
            IdProduto = produto.Id,
            NomeProduto = produto.Nome,
            IdUnidade = unidade.Id,
            Quantidade = estoqueCriado.Quantidade,
            QuantidadeMinima = estoqueCriado.QuantidadeMinima,
            Disponivel = estoqueCriado.EstaDisponivel(),
            DataInsercao = estoqueCriado.DataInsercao
        };

        return ResultViewModel<CriarEstoqueResponse>.Success(response);
    }
}