using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Campanhas.AlterarStatusCampanha;

public class AlterarStatusCampanhaHandler(ICampanhaRepository _repository) : IRequestHandler<AlterarStatusCampanhaCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(
        AlterarStatusCampanhaCommand command, CancellationToken cancellationToken)
    {
        var campanha = await _repository.ObterPorIdAsync(command.Id);

        if (campanha == null)
            return ResultViewModel<bool>.Error("Campanha não encontrada.");

        switch (command.NovoStatus)
        {
            case StatusCampanha.Ativa:
                campanha.Ativar();
                break;
            case StatusCampanha.Inativa:
                campanha.Desativar();
                break;
            case StatusCampanha.Encerrada:
                campanha.Encerrar();
                break;
            default:
                return ResultViewModel<bool>.Error("Status informado não é válido.");
        }

        await _repository.AtualizarAsync(campanha);

        return ResultViewModel<bool>.Success(true, $"Campanha {command.NovoStatus.ToString().ToLower()} com sucesso.");
    }
}