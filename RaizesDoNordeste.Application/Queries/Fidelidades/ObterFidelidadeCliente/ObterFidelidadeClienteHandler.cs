using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Fidelidades.ObterFidelidadeCliente;

public class ObterFidelidadeClienteHandler(IFidelidadeRepository _repository)
    : IRequestHandler<ObterFidelidadeClienteQuery, ResultViewModel<FidelidadeQueryResponse>>
{
    public async Task<ResultViewModel<FidelidadeQueryResponse>> Handle(
        ObterFidelidadeClienteQuery query, CancellationToken cancellationToken)
    {
        var fidelidade = await _repository.ObterPorClienteIdAsync(query.IdCliente);

        if (fidelidade == null)
            return ResultViewModel<FidelidadeQueryResponse>.Error("Cliente não está cadastrado no programa de fidelidade.");

        var response = new FidelidadeQueryResponse
        {
            Id = fidelidade.Id,
            IdCliente = fidelidade.IdCliente,
            NomeCliente = fidelidade.Cliente?.NomeCompleto ?? "",
            SaldoPontos = fidelidade.SaldoPontos,
            Nivel = fidelidade.Nivel.ToString(),
            ValorEmDesconto = fidelidade.SaldoPontos * 0.10m
        };

        return ResultViewModel<FidelidadeQueryResponse>.Success(response);
    }
}
