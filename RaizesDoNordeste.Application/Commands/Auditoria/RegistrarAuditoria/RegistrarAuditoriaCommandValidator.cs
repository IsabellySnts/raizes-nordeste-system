using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Auditoria.RegistrarAuditoria;

public class RegistrarAuditoriaCommandValidator : AbstractValidator<RegistrarAuditoriaCommand>
{
    public RegistrarAuditoriaCommandValidator()
    {
        RuleFor(x => x.IdFuncionario)
            .GreaterThan(0).WithMessage("O funcionário é obrigatório.");

        RuleFor(x => x.Acao)
            .IsInEnum().WithMessage("A ação informada não é válida.");

        RuleFor(x => x.TipoEntidadeAfetada)
            .NotEmpty().WithMessage("O tipo da entidade afetada é obrigatório.")
            .MaximumLength(50).WithMessage("O tipo da entidade deve ter no máximo 50 caracteres.");

        RuleFor(x => x.IdEntidadeAfetada)
            .GreaterThan(0).WithMessage("O Id da entidade afetada é obrigatório.");

        RuleFor(x => x.Detalhes)
            .MaximumLength(2000).WithMessage("Os detalhes devem ter no máximo 2000 caracteres.")
            .When(x => x.Detalhes != null);
    }
}
