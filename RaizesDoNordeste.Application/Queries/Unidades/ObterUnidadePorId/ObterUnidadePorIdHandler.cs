using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Queries.Unidades.ObterUnidadePorId;

public class ObterUnidadePorIdHandler(IUnidadeRepository _repository): IRequestHandler<ObterUnidadePorIdQuery, ResultViewModel<UnidadeQueryResponse>>
{
    public async Task<ResultViewModel<UnidadeQueryResponse>> Handle(ObterUnidadePorIdQuery query, CancellationToken cancellationToken)
    {
        var unidade = await _repository.ObterPorIdComDetalhesAsync(query.Id);

        if (unidade == null)
            return ResultViewModel<UnidadeQueryResponse>.Error("Unidade não encontrada.");

        var response = new UnidadeQueryResponse
        {
            Id = unidade.Id,
            Nome = unidade.Nome,
            Cidade = unidade.Cidade,
            Estado = unidade.Estado,
            Pais = unidade.Pais,
            Logradouro = unidade.Logradouro,
            Complemento = unidade.Complemento,
            DiasFuncionamento = unidade.DiasFuncionamento,
            HorarioFuncionamento = unidade.HorarioFuncionamento,
            TipoCozinha = unidade.TipoCozinha.ToString(),
            TotalFuncionarios = unidade.Funcionarios.Count
        };

        return ResultViewModel<UnidadeQueryResponse>.Success(response);
    }
}