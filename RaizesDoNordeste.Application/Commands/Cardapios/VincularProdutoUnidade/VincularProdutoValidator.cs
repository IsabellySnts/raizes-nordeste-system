using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Cardapios.VincularProdutoUnidade;

public class VincularProdutoCommandValidator : AbstractValidator<VincularProdutoCommand>
{
    public VincularProdutoCommandValidator()
    {
        RuleFor(x => x.IdUnidade)
            .GreaterThan(0).WithMessage("A unidade é obrigatória.");

        RuleFor(x => x.IdProduto)
            .GreaterThan(0).WithMessage("O produto é obrigatório.");

        RuleFor(x => x.PrecoLocal)
            .GreaterThan(0).WithMessage("O preço local deve ser maior que zero.")
            .When(x => x.PrecoLocal.HasValue);

        RuleFor(x => x.VariacaoRegional)
            .MaximumLength(500).WithMessage("A variação regional deve ter no máximo 500 caracteres.")
            .When(x => x.VariacaoRegional != null);
    }
}