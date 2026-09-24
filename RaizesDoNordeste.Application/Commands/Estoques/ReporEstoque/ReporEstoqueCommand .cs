using MediatR;
using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Commands.Estoques.ReporEstoque;

public sealed record ReporEstoqueCommand : IRequest<ResultViewModel<EstoqueAtualizadoResponse>>
{
    public long IdUnidade { get; init; }
    public long IdProduto { get; init; }
    public int Quantidade { get; init; }
}
