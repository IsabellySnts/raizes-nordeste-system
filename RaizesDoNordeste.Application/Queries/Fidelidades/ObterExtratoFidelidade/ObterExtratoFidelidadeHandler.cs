using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Fidelidades.ObterExtratoFidelidade;

public class ObterExtratoFidelidadeHandler(IFidelidadeRepository _repository) : IRequestHandler<ObterExtratoFidelidadeQuery, ResultViewModel<ExtratoFidelidadeResponse>>
{
    public async Task<ResultViewModel<ExtratoFidelidadeResponse>> Handle(
        ObterExtratoFidelidadeQuery query, CancellationToken cancellationToken)
    {
        var fidelidade = await _repository.ObterPorClienteIdComMovimentacoesAsync(query.IdCliente);

        if (fidelidade == null)
            return ResultViewModel<ExtratoFidelidadeResponse>.Error("Cliente não está cadastrado no programa de fidelidade.");

        var response = new ExtratoFidelidadeResponse
        {
            IdCliente = fidelidade.IdCliente,
            NomeCliente = fidelidade.Cliente?.NomeCompleto ?? "",
            SaldoAtual = fidelidade.SaldoPontos,
            Nivel = fidelidade.Nivel.ToString(),
            Movimentacoes = fidelidade.Movimentacoes.Select(m => new MovimentacaoQueryResponse
            {
                Id = m.Id,
                Tipo = m.Tipo.ToString(),
                Pontos = m.Pontos,
                IdPedido = m.IdPedido,
                Data = m.Data
            })
        };

        return ResultViewModel<ExtratoFidelidadeResponse>.Success(response);
    }
}
