using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Campanhas.CriarCampanha;

public class CriarCampanhaCommandValidator : AbstractValidator<CriarCampanhaCommand>
{
    public CriarCampanhaCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da campanha é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.")
            .When(x => x.Descricao != null);

        RuleFor(x => x.Criterios)
            .MaximumLength(2000).WithMessage("Os critérios devem ter no máximo 2000 caracteres.")
            .When(x => x.Criterios != null);

        RuleFor(x => x.DataInicio)
            .NotEmpty().WithMessage("A data de início é obrigatória.");

        RuleFor(x => x.DataFim)
            .NotEmpty().WithMessage("A data de fim é obrigatória.")
            .GreaterThan(x => x.DataInicio).WithMessage("A data de fim deve ser posterior à data de início.");

        RuleFor(x => x.Beneficio)
            .NotEmpty().WithMessage("O benefício é obrigatório.")
            .MaximumLength(200).WithMessage("O benefício deve ter no máximo 200 caracteres.");
    }
}
