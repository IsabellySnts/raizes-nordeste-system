using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Consentimento.ObterConsentimentoCliente;

public class ObterConsentimentosClienteHandler(IConsentimentoLGPDRepository _repository, IClienteRepository _clienteRepository) : IRequestHandler<ObterConsentimentosClienteQuery, ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>> Handle(ObterConsentimentosClienteQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(query.IdCliente);

            if (cliente == null)
                return ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>.Error("Cliente não encontrado.", ResultStatus.NotFound);

            if (query.PerfilLogado == PerfilUsuario.Cliente && cliente.IdUsuario != query.IdUsuarioLogado)
                return ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>.Error("Você não tem permissão para consultar consentimentos de outro cliente.", ResultStatus.Forbidden);

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
        catch(Exception ex)
        {
            return ResultViewModel<IEnumerable<ConsentimentoQueryResponse>>.Error($"Ocorreu um erro ao obter os consentimentos do cliente: {ex.Message}", ResultStatus.InternalServerError);
        }
    }
}
