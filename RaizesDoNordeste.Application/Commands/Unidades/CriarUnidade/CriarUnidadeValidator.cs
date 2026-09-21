using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Unidades.CriarUnidade;

internal class CriarUnidadeValidator : AbstractValidator<CriarUnidadeCommand>
{
    public CriarUnidadeValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da unidade é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.")
            .MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("O estado é obrigatório.")
            .MaximumLength(50).WithMessage("O estado deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Pais)
            .NotEmpty().WithMessage("O país é obrigatório.")
            .MaximumLength(50).WithMessage("O país deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("O logradouro é obrigatório.")
            .MaximumLength(300).WithMessage("O logradouro deve ter no máximo 300 caracteres.");

        RuleFor(x => x.Complemento)
            .MaximumLength(200).WithMessage("O complemento deve ter no máximo 200 caracteres.")
            .When(x => x.Complemento != null);

        RuleFor(x => x.DiasFuncionamento)
            .NotEmpty().WithMessage("Os dias de funcionamento são obrigatórios.")
            .MaximumLength(100).WithMessage("Os dias de funcionamento devem ter no máximo 100 caracteres.");

        RuleFor(x => x.HorarioFuncionamento)
            .NotEmpty().WithMessage("O horário de funcionamento é obrigatório.")
            .MaximumLength(50).WithMessage("O horário deve ter no máximo 50 caracteres.");

        RuleFor(x => x.TipoCozinha)
            .IsInEnum().WithMessage("O tipo de cozinha informado não é válido.");
    }
}