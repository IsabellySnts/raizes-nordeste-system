using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Cardapios.ObterCardapioUnidade;

public class ObterCardapioUnidadeHandler(
    ICardapioRepository _cardapioRepository,
    IUnidadeRepository _unidadeRepository)
    : IRequestHandler<ObterCardapioUnidadeQuery, ResultViewModel<IEnumerable<CardapioQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<CardapioQueryResponse>>> Handle(
        ObterCardapioUnidadeQuery query, CancellationToken cancellationToken)
    {
        var unidade = await _unidadeRepository.ObterPorIdAsync(query.IdUnidade);
        if (unidade == null)
            return ResultViewModel<IEnumerable<CardapioQueryResponse>>.Error("Unidade não encontrada.");

        var itens = query.ApenasDisponiveis
            ? await _cardapioRepository.ObterDisponivelPorUnidadeAsync(query.IdUnidade)
            : await _cardapioRepository.ObterPorUnidadeAsync(query.IdUnidade);

        var response = itens.Select(c => new CardapioQueryResponse
        {
            Id = c.Id,
            IdProduto = c.IdProduto,
            NomeProduto = c.Produto?.Nome ?? "",
            DescricaoProduto = c.Produto?.Descricao,
            CategoriaProduto = c.Produto?.Categoria?.Nome,
            PrecoExibido = c.PrecoLocal ?? c.Produto?.Preco ?? 0,
            Disponivel = c.Disponivel,
            VariacaoRegional = c.VariacaoRegional,
            Foto = c.Produto?.Foto,
            Sazonal = c.Produto?.FlagSazonal ?? false
        });

        return ResultViewModel<IEnumerable<CardapioQueryResponse>>.Success(response);
    }
}