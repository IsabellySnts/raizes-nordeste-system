using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Cardapios.AtualizarCardapio;

public class AtualizarCardapioHandler(ICardapioRepository _repository)
    : IRequestHandler<AtualizarCardapioCommand, ResultViewModel<AtualizarCardapioResponse>>
{
    public async Task<ResultViewModel<AtualizarCardapioResponse>> Handle(
        AtualizarCardapioCommand command, CancellationToken cancellationToken)
    {
        var validator = new AtualizarCardapioCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AtualizarCardapioResponse>.Error(erros);
        }

        var cardapio = await _repository.ObterPorIdComDetalhesAsync(command.Id);
        if (cardapio == null)
            return ResultViewModel<AtualizarCardapioResponse>.Error("Item do cardápio não encontrado.");

        cardapio.AtualizarDisponibilidade(command.Disponivel);
        cardapio.AtualizarPrecoLocal(command.PrecoLocal);
        cardapio.AtualizarVariacaoRegional(command.VariacaoRegional);

        await _repository.AtualizarAsync(cardapio);

        var response = new AtualizarCardapioResponse
        {
            Id = cardapio.Id,
            IdProduto = cardapio.IdProduto,
            NomeProduto = cardapio.Produto?.Nome ?? "",
            IdUnidade = cardapio.IdUnidade,
            NomeUnidade = cardapio.Unidade?.Nome ?? "",
            Disponivel = cardapio.Disponivel,
            PrecoExibido = command.PrecoLocal ?? cardapio.Produto?.Preco ?? 0,
            VariacaoRegional = cardapio.VariacaoRegional
        };

        return ResultViewModel<AtualizarCardapioResponse>.Success(response);
    }
}