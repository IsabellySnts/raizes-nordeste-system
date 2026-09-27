using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;

public class AcumularPontosCommandValidator : AbstractValidator<AcumularPontosCommand>
{
    public AcumularPontosCommandValidator()
    {
        RuleFor(x => x.IdCliente)
            .GreaterThan(0).WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.ValorPedido)
            .GreaterThan(0).WithMessage("O valor do pedido deve ser maior que zero.");
    }
}
