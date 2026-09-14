using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Categorias.CriarCategoria;

public class CriarCategoriaValidator : AbstractValidator<CriarCategoriaCommand>
{
    public CriarCategoriaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da categoria não pode exceder 100 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500).WithMessage("A descrição da categoria não pode exceder 500 caracteres.");
    }
}