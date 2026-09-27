using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.ConsentimentoLGPD.ObterConsentimentoCliente;

public class ObterConsentimentosClienteHandler(IConsentimentoLGPDRepository _repository)
    : IRequestHandler<ObterConsentimentosClienteQuery, ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>> Handle(
        ObterConsentimentosClienteQuery query, CancellationToken cancellationToken)
    {
        var consentimentos = await _repository.ObterPorClienteAsync(query.IdCliente);

        var response = consentimentos.Select(c => new ConsentimentoQueryResponse
        {
            Id = c.Id,
            Permissao = c.Permissao.ToString(),
            Aceite = c.Aceite,
            Data = c.Data
        });

        return ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>.Success(response);
    }
}
