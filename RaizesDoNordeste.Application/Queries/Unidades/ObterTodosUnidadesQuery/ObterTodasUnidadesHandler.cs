using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Unidades.ObterTodosUnidadesQuery;

public class ObterTodasUnidadesHandler(IUnidadeRepository _repository) : IRequestHandler<ObterTodasUnidadesQuery, ResultViewModel<IEnumerable<UnidadeQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<UnidadeQueryResponse>>> Handle(
        ObterTodasUnidadesQuery query, CancellationToken cancellationToken)
    {
        var unidades = await _repository.ObterTodasAsync();

        var response = unidades.Select(u => new UnidadeQueryResponse
        {
            Id = u.Id,
            Nome = u.Nome,
            Cidade = u.Cidade,
            Estado = u.Estado,
            Pais = u.Pais,
            Logradouro = u.Logradouro,
            Complemento = u.Complemento,
            DiasFuncionamento = u.DiasFuncionamento,
            HorarioFuncionamento = u.HorarioFuncionamento,
            TipoCozinha = u.TipoCozinha.ToString()
        });

        return ResultViewModel<IEnumerable<UnidadeQueryResponse>>.Success(response);
    }
}
