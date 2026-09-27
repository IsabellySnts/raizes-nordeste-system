using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.ResgatarPontos;

public class ResgatarPontosCommandValidator : AbstractValidator<ResgatarPontosCommand>
{
    public ResgatarPontosCommandValidator()
    {
        RuleFor(x => x.IdCliente)
            .GreaterThan(0).WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.Pontos)
            .GreaterThan(0).WithMessage("A quantidade de pontos deve ser maior que zero.");
    }
}
