using MediatR;
using RaizesDoNordeste.Application.Commands.Auditoria.RegistrarAuditoria;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Estoques.AjustarEstoque;

public class AjustarEstoqueHandler(IEstoqueRepository _repository, IMediator _mediator) : IRequestHandler<AjustarEstoqueCommand, ResultViewModel<AjustarEstoqueResponse>>
{
    public async Task<ResultViewModel<AjustarEstoqueResponse>> Handle(
        AjustarEstoqueCommand command, CancellationToken cancellationToken)
    {
        var validator = new AjustarEstoqueCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AjustarEstoqueResponse>.Error(erros);
        }

        var estoque = await _repository.ObterPorProdutoUnidadeAsync(command.IdProduto, command.IdUnidade);
        if (estoque == null)
            return ResultViewModel<AjustarEstoqueResponse>.Error("Registro de estoque não encontrado para este produto nesta unidade.");

        var quantidadeAnterior = estoque.Quantidade;

        estoque.Ajustar(command.NovaQuantidade, command.NovaQuantidadeMinima);
        await _repository.AtualizarAsync(estoque);

        await _mediator.Send(new RegistrarAuditoriaCommand
        {
            IdFuncionario = command.IdFuncionario,
            Acao = AcaoAuditoria.AjusteEstoque,
            TipoEntidadeAfetada = "Estoque",
            IdEntidadeAfetada = estoque.Id,
            Detalhes = $"Ajuste de {command.NovaQuantidade} unidades no estoque #{estoque.Id} na unidade #{command.IdUnidade}"
        }, cancellationToken);

        var response = new AjustarEstoqueResponse
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

        return ResultViewModel<AjustarEstoqueResponse>.Success(response);
    }
}
