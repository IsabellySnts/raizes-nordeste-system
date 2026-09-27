using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Auditorias.ObterAuditorias;

public class ObterAuditoriasHandler(IAuditoriaRepository _repository) : IRequestHandler<ObterAuditoriasQuery, ResultViewModel<IEnumerable<AuditoriaQueryResponse>>>
{
    public async Task<ResultViewModel<IEnumerable<AuditoriaQueryResponse>>> Handle(
        ObterAuditoriasQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Auditoria> auditorias;

        if (query.TipoEntidade != null && query.IdEntidade.HasValue)
            auditorias = await _repository.ObterPorEntidadeAsync(query.TipoEntidade, query.IdEntidade.Value);
        else if (query.Acao.HasValue)
            auditorias = await _repository.ObterPorAcaoAsync(query.Acao.Value);
        else if (query.IdFuncionario.HasValue)
            auditorias = await _repository.ObterPorFuncionarioAsync(query.IdFuncionario.Value);
        else
            auditorias = await _repository.ObterTodosAsync();

        var response = auditorias.Select(a => new AuditoriaQueryResponse
        {
            Id = a.Id,
            IdFuncionario = a.IdFuncionario,
            NomeFuncionario = a.Funcionario?.Nome ?? "",
            CargoFuncionario = a.Funcionario?.Cargo.ToString(),
            Acao = a.Acao.ToString(),
            TipoEntidadeAfetada = a.TipoEntidadeAfetada,
            IdEntidadeAfetada = a.IdEntidadeAfetada,
            Detalhes = a.Detalhes,
            DataHora = a.DataHora
        });

        return ResultViewModel<IEnumerable<AuditoriaQueryResponse>>.Success(response);
    }
}
