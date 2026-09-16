using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Categorias.AlterarCategoria;

public class AlterarCategoriaValidator : AbstractValidator<AlterarCategoriaCommand>
{
    public AlterarCategoriaValidator()
    {
        RuleFor(x => x.IdCategoria)
            .NotEmpty().WithMessage("O id da categoria é obrigatório.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
            .When(x => x.Descricao != null);
    }
}
