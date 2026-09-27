using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;

public class RegistrarConsentimentoCommandValidator : AbstractValidator<RegistrarConsentimentoCommand>
{
    public RegistrarConsentimentoCommandValidator()
    {
        RuleFor(x => x.IdCliente)
            .GreaterThan(0).WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.Permissao)
            .IsInEnum().WithMessage("O tipo de consentimento informado não é válido.");
    }
}
