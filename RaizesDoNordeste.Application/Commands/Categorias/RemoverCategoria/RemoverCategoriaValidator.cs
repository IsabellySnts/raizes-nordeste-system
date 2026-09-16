using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Categorias.RemoverCategoria;

public class RemoverCategoriaCommandValidator : AbstractValidator<RemoverCategoriaCommand>
{
    public RemoverCategoriaCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O Id da categoria é obrigatório.");
    }
}