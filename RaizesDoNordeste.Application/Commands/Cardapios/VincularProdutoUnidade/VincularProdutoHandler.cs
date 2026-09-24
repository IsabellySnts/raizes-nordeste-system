using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Cardapios.VincularProdutoUnidade;

public class VincularProdutoHandler(
    ICardapioRepository _cardapioRepository,
    IProdutoRepository _produtoRepository,
    IUnidadeRepository _unidadeRepository)
    : IRequestHandler<VincularProdutoCommand, ResultViewModel<VincularProdutoResponse>>
{
    public async Task<ResultViewModel<VincularProdutoResponse>> Handle(
        VincularProdutoCommand command, CancellationToken cancellationToken)
    {
        var validator = new VincularProdutoCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<VincularProdutoResponse>.Error(erros);
        }

        var produto = await _produtoRepository.ObterPorIdAsync(command.IdProduto);
        if (produto == null)
            return ResultViewModel<VincularProdutoResponse>.Error("Produto não encontrado.");

        var unidade = await _unidadeRepository.ObterPorIdAsync(command.IdUnidade);
        if (unidade == null)
            return ResultViewModel<VincularProdutoResponse>.Error("Unidade não encontrada.");

        var jaVinculado = await _cardapioRepository.ProdutoJaVinculadoAsync(command.IdProduto, command.IdUnidade);
        if (jaVinculado)
            return ResultViewModel<VincularProdutoResponse>.Error("Este produto já está vinculado a esta unidade.");

        var cardapio = new Cardapio(
            command.IdProduto,
            command.IdUnidade,
            command.Disponivel,
            command.PrecoLocal,
            command.VariacaoRegional
        );

        var cardapioCriado = await _cardapioRepository.CriarAsync(cardapio);

        var response = new VincularProdutoResponse
        {
            Id = cardapioCriado.Id,
            IdProduto = produto.Id,
            NomeProduto = produto.Nome,
            IdUnidade = unidade.Id,
            NomeUnidade = unidade.Nome,
            Disponivel = cardapioCriado.Disponivel,
            PrecoExibido = command.PrecoLocal ?? produto.Preco,
            VariacaoRegional = cardapioCriado.VariacaoRegional
        };

        return ResultViewModel<VincularProdutoResponse>.Success(response);
    }
}
