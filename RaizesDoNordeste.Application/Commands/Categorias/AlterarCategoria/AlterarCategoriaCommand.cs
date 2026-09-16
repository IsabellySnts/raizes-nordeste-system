using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Categorias.AlterarCategoria
{
    public sealed record AlterarCategoriaCommand : IRequest<ResultViewModel<AlterarCategoriaResponse>>
    {
        public long IdCategoria { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}
