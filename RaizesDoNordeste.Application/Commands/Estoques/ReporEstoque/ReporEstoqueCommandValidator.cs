using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Estoques.ReporEstoque;

public class ReporEstoqueCommandValidator : AbstractValidator<ReporEstoqueCommand>
{
    public ReporEstoqueCommandValidator()
    {
        RuleFor(x => x.IdUnidade)
            .GreaterThan(0).WithMessage("A unidade é obrigatória.");

        RuleFor(x => x.IdProduto)
            .GreaterThan(0).WithMessage("O produto é obrigatório.");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0).WithMessage("A quantidade a repor deve ser maior que zero.");
    }
}
