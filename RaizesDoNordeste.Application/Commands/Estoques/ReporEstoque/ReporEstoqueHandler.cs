using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Estoques.ReporEstoque;

public class ReporEstoqueHandler(IEstoqueRepository _repository)
    : IRequestHandler<ReporEstoqueCommand, ResultViewModel<EstoqueAtualizadoResponse>>
{
    public async Task<ResultViewModel<EstoqueAtualizadoResponse>> Handle(
        ReporEstoqueCommand command, CancellationToken cancellationToken)
    {
        var validator = new ReporEstoqueCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<EstoqueAtualizadoResponse>.Error(erros);
        }

        var estoque = await _repository.ObterPorProdutoUnidadeAsync(command.IdProduto, command.IdUnidade);
        if (estoque == null)
            return ResultViewModel<EstoqueAtualizadoResponse>.Error("Registro de estoque não encontrado para este produto nesta unidade.");

        var quantidadeAnterior = estoque.Quantidade;

        estoque.Repor(command.Quantidade);
        await _repository.AtualizarAsync(estoque);

        var response = new EstoqueAtualizadoResponse
        {
            Id = estoque.Id,
            IdProduto = estoque.IdProduto,
            NomeProduto = estoque.Produto?.Nome ?? "",
            IdUnidade = estoque.IdUnidade,
            QuantidadeAnterior = quantidadeAnterior,
            QuantidadeAtual = estoque.Quantidade,
            QuantidadeMinima = estoque.QuantidadeMinima,
            Disponivel = estoque.EstaDisponivel(),
            DataAtualizacao = estoque.DataAtualizacao
        };

        return ResultViewModel<EstoqueAtualizadoResponse>.Success(response);
    }
}