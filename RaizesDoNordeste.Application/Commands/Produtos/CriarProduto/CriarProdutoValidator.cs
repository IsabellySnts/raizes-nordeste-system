using FluentValidation;

namespace RaizesDoNordeste.Application.Commands.Produtos.CriarProduto;

public class CriarProdutoValidator : AbstractValidator<CriarProdutoCommand>
{
    public CriarProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.IdCategoria)
            .GreaterThan(0).WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");

        RuleFor(x => x.Descricao)
            .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.")
            .When(x => x.Descricao != null);

        RuleFor(x => x.DataInicioDisponibilidade)
            .NotNull().WithMessage("Data de início é obrigatória para produtos sazonais.")
            .When(x => x.FlagSazonal);

        RuleFor(x => x.DataFimDisponibilidade)
            .NotNull().WithMessage("Data de fim é obrigatória para produtos sazonais.")
            .GreaterThan(x => x.DataInicioDisponibilidade)
            .WithMessage("Data de fim deve ser posterior à data de início.")
            .When(x => x.FlagSazonal);

        RuleFor(x => x.Foto)
            .MaximumLength(500).WithMessage("A URL da foto deve ter no máximo 500 caracteres.")
            .When(x => x.Foto != null);
    }
}