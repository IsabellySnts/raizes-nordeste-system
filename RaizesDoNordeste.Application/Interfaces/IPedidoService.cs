using RaizesDoNordeste.Application.Commons;

namespace RaizesDoNordeste.Application.Interfaces;

public interface IPedidoService
{
    Task<ResultViewModel<ItemValidado>> ValidarItemAsync(long idProduto, long idUnidade, int quantidade);
    Task<ResultViewModel<DescontoAplicado>> ResgatarPontosAsync(long idCliente, int pontos, decimal valorTotal);
    Task ReduzirEstoqueAsync(long idProduto, long idUnidade, int quantidade);
}

public record ItemValidado(decimal PrecoUnitario, string NomeProduto);
public record DescontoAplicado(int PontosResgatados, decimal ValorDesconto);