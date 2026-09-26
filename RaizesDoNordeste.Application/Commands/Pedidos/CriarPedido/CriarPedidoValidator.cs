using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Pedidos.CriarPedido;

public class CriarPedidoCommandValidator : AbstractValidator<CriarPedidoCommand>
{
    public CriarPedidoCommandValidator()
    {
        RuleFor(x => x.IdUnidade)
            .GreaterThan(0).WithMessage("A unidade é obrigatória.");

        RuleFor(x => x.CanalOrigem)
            .IsInEnum().WithMessage("O canal de origem não é válido.");

        RuleFor(x => x.Itens)
            .NotEmpty().WithMessage("O pedido deve ter pelo menos um item.");

        RuleForEach(x => x.Itens).ChildRules(item =>
        {
            item.RuleFor(i => i.IdProduto)
                .GreaterThan(0).WithMessage("O produto é obrigatório.");

            item.RuleFor(i => i.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            item.RuleFor(i => i.Observacao)
                .MaximumLength(500).WithMessage("A observação deve ter no máximo 500 caracteres.")
                .When(i => i.Observacao != null);
        });
    }
}
