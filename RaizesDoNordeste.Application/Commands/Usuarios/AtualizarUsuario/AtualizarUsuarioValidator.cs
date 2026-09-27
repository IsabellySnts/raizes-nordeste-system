using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Usuarios.AtualizarUsuario;

public class AtualizarUsuarioCommandValidator : AbstractValidator<AtualizarUsuarioCommand>
{
    public AtualizarUsuarioCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O Id do usuário é obrigatório.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email informado não é válido.")
            .MaximumLength(200).WithMessage("O email deve ter no máximo 200 caracteres.");

        RuleFor(x => x.NovaSenha)
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.")
            .Matches("[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches("[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches("[0-9]").WithMessage("A senha deve conter pelo menos um número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("A senha deve conter pelo menos um caractere especial.")
            .When(x => !string.IsNullOrEmpty(x.NovaSenha));

        RuleFor(x => x.ConfirmarNovaSenha)
            .Equal(x => x.NovaSenha).WithMessage("A confirmação de senha não corresponde.")
            .When(x => !string.IsNullOrEmpty(x.NovaSenha));
    }
}
