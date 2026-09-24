using FluentValidation;
using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commands.Funcionarios.CriarFuncionario;

public class CriarFuncionarioCommandValidator : AbstractValidator<CriarFuncionarioCommand>
{
    public CriarFuncionarioCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Matches(@"^\d{11}$").WithMessage("O CPF deve conter exatamente 11 dígitos numéricos.");

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

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .Matches("[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches("[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches("[0-9]").WithMessage("A senha deve conter pelo menos um número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("A senha deve conter pelo menos um caractere especial.");

        RuleFor(x => x.ConfirmarSenha)
            .Equal(x => x.Senha).WithMessage("A confirmação de senha não corresponde.");
    }
}