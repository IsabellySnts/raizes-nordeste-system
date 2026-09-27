using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Campanhas.ObterCampanhaPorId;

public class ObterCampanhaPorIdHandler(ICampanhaRepository _repository) : IRequestHandler<ObterCampanhaPorIdQuery, ResultViewModel<CampanhaQueryResponse>>
{
    public async Task<ResultViewModel<CampanhaQueryResponse>> Handle(
        ObterCampanhaPorIdQuery query, CancellationToken cancellationToken)
    {
        var campanha = await _repository.ObterPorIdAsync(query.Id);

        if (campanha == null)
            return ResultViewModel<CampanhaQueryResponse>.Error("Campanha não encontrada.");

        var response = new CampanhaQueryResponse
        {
            Id = campanha.Id,
            Nome = campanha.Nome,
            Descricao = campanha.Descricao,
            Criterios = campanha.Criterios,
            DataInicio = campanha.DataInicio,
            DataFim = campanha.DataFim,
            Status = campanha.Status.ToString(),
            Beneficio = campanha.Beneficio,
            AtivaAgora = campanha.EstaAtiva(DateTime.UtcNow)
        };

        return ResultViewModel<CampanhaQueryResponse>.Success(response);
    }
}
