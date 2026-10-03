using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Application.Interfaces;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Services;

public class PedidoService(
    ICardapioRepository _cardapioRepository,
    IEstoqueRepository _estoqueRepository,
    IFidelidadeRepository _fidelidadeRepository) : IPedidoService
{
    public async Task<ResultViewModel<ItemValidado>> ValidarItemAsync(long idProduto, long idUnidade, int quantidade)
    {
        var cardapio = await _cardapioRepository.ObterPorProdutoUnidadeAsync(idProduto, idUnidade);

        if (cardapio == null || !cardapio.Disponivel)
            return ResultViewModel<ItemValidado>.Error($"Produto {idProduto} não está disponível no cardápio desta unidade.");

        var estoque = await _estoqueRepository.ObterPorProdutoUnidadeAsync(idProduto, idUnidade);

        if (estoque == null || !estoque.EstaDisponivel())
            return ResultViewModel<ItemValidado>.Error($"Produto {idProduto} está com estoque indisponível.");

        if (estoque.Quantidade < quantidade)
            return ResultViewModel<ItemValidado>.Error($"Estoque insuficiente para o produto {idProduto}. Disponível: {estoque.Quantidade}.");

        var preco = cardapio.PrecoLocal ?? cardapio.Produto.Preco;
        var nome = cardapio.Produto?.Nome ?? "";

        return ResultViewModel<ItemValidado>.Success(new ItemValidado(preco, nome));
    }

    public async Task ReduzirEstoqueAsync(long idProduto, long idUnidade, int quantidade)
    {
        var estoque = await _estoqueRepository.ObterPorProdutoUnidadeAsync(idProduto, idUnidade);
        estoque!.Reduzir(quantidade);
        await _estoqueRepository.AtualizarAsync(estoque);
    }

    public async Task<ResultViewModel<DescontoAplicado>> ResgatarPontosAsync(long idCliente, int pontos, decimal valorTotal)
    {
        var fidelidade = await _fidelidadeRepository.ObterPorClienteIdAsync(idCliente);

        if (fidelidade == null)
            return ResultViewModel<DescontoAplicado>.Error("Cliente não está cadastrado no programa de fidelidade.");

        var pontosResgatados = pontos;
        var valorDesconto = pontosResgatados * 0.10m;

        if (valorDesconto > valorTotal)
        {
            pontosResgatados = (int)Math.Ceiling(valorTotal / 0.10m);
            valorDesconto = valorTotal;
        }

        var resgatou = fidelidade.ResgatarPontos(pontosResgatados, null);
        if (!resgatou)
            return ResultViewModel<DescontoAplicado>.Error($"Saldo insuficiente. Saldo atual: {fidelidade.SaldoPontos} pontos.");

        await _fidelidadeRepository.AtualizarAsync(fidelidade);

        return ResultViewModel<DescontoAplicado>.Success(new DescontoAplicado(pontosResgatados, valorDesconto));
    }
}
