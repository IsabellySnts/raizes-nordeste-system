using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Estoques.AjustarEstoque;

public class AjustarEstoqueCommandValidator : AbstractValidator<AjustarEstoqueCommand>
{
    public AjustarEstoqueCommandValidator()
    {
        RuleFor(x => x.IdUnidade)
            .GreaterThan(0).WithMessage("A unidade é obrigatória.");

        RuleFor(x => x.IdProduto)
            .GreaterThan(0).WithMessage("O produto é obrigatório.");

        RuleFor(x => x.NovaQuantidade)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade não pode ser negativa.");

        RuleFor(x => x.NovaQuantidadeMinima)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade mínima não pode ser negativa.")
            .When(x => x.NovaQuantidadeMinima.HasValue);
    }
}
