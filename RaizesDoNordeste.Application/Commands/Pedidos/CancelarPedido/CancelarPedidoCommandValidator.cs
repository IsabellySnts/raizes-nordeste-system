using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CancelarPedido;

public class CancelarPedidoCommandValidator : AbstractValidator<CancelarPedidoCommand>
{
    public CancelarPedidoCommandValidator()
    {
        RuleFor(x => x.IdPedido)
            .GreaterThan(0).WithMessage("O Id do pedido é obrigatório.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("O motivo do cancelamento é obrigatório.")
            .MaximumLength(1000).WithMessage("O motivo deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.IdFuncionario)
            .NotNull().WithMessage("O Id do funcionário é obrigatório para cancelamento pelo gerente.")
            .GreaterThan(0).WithMessage("O Id do funcionário não é válido.")
            .When(x => !x.CanceladoPeloCliente);
    }
}
