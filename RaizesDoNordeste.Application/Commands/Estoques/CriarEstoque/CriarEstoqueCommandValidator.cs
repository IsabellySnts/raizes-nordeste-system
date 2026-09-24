using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Estoques.CriarEstoque;

public class CriarEstoqueCommandValidator : AbstractValidator<CriarEstoqueCommand>
{
    public CriarEstoqueCommandValidator()
    {
        RuleFor(x => x.IdUnidade)
            .GreaterThan(0).WithMessage("A unidade é obrigatória.");

        RuleFor(x => x.IdProduto)
            .GreaterThan(0).WithMessage("O produto é obrigatório.");

        RuleFor(x => x.Quantidade)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade não pode ser negativa.");

        RuleFor(x => x.QuantidadeMinima)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade mínima não pode ser negativa.");
    }
}