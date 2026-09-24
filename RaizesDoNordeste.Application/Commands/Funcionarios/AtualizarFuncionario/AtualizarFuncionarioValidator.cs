using FluentValidation;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.AtualizarFuncionario;

public class AtualizarFuncionarioCommandValidator : AbstractValidator<AtualizarFuncionarioCommand>
{
    public AtualizarFuncionarioCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O Id do funcionário é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email informado não é válido.")
            .MaximumLength(200).WithMessage("O email deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Cargo)
            .IsInEnum().WithMessage("O cargo informado não é válido.");

        RuleFor(x => x.IdUnidade)
            .NotNull().WithMessage("A unidade é obrigatória para este cargo.")
            .GreaterThan(0).WithMessage("A unidade informada não é válida.")
            .When(x => x.Cargo != CargoFuncionario.GestorMatriz);

        RuleFor(x => x.IdUnidade)
            .Null().WithMessage("Gestor da matriz não deve estar vinculado a uma unidade.")
            .When(x => x.Cargo == CargoFuncionario.GestorMatriz);
    }
}
