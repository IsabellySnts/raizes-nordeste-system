using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Cardapios.AtualizarCardapio;

public class AtualizarCardapioCommandValidator : AbstractValidator<AtualizarCardapioCommand>
{
    public AtualizarCardapioCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O Id do cardápio é obrigatório.");

        RuleFor(x => x.PrecoLocal)
            .GreaterThan(0).WithMessage("O preço local deve ser maior que zero.")
            .When(x => x.PrecoLocal.HasValue);

        RuleFor(x => x.VariacaoRegional)
            .MaximumLength(500).WithMessage("A variação regional deve ter no máximo 500 caracteres.")
            .When(x => x.VariacaoRegional != null);
    }
}