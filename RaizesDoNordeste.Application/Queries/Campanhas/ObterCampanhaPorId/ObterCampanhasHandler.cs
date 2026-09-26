using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Campanhas.ObterCampanhaPorId;

public class ObterCampanhasHandler(ICampanhaRepository _repository) : IRequestHandler<ObterCampanhasQuery, ResultViewModel<IEnumerable<CampanhaQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<CampanhaQueryResponse>>> Handle(
        ObterCampanhasQuery query, CancellationToken cancellationToken)
    {
        var campanhas = query.ApenasAtivas
            ? await _repository.ObterAtivasAsync()
            : await _repository.ObterTodasAsync();

        var agora = DateTime.UtcNow;

        var response = campanhas.Select(c => new CampanhaQueryResponse
        {
            Id = c.Id,
            Nome = c.Nome,
            Descricao = c.Descricao,
            Criterios = c.Criterios,
            DataInicio = c.DataInicio,
            DataFim = c.DataFim,
            Status = c.Status.ToString(),
            Beneficio = c.Beneficio,
            AtivaAgora = c.EstaAtiva(agora)
        });

        return ResultViewModel<IEnumerable<CampanhaQueryResponse>>.Success(response);
    }
}
