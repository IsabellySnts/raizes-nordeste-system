using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;

public class RevogarConsentimentoHandler(IConsentimentoLGPDRepository _repository)
    : IRequestHandler<RevogarConsentimentoCommand, ResultViewModel<RevogarConsentimentoResponse>>
{
    public async Task<ResultViewModel<RevogarConsentimentoResponse>> Handle(
        RevogarConsentimentoCommand command, CancellationToken cancellationToken)
    {
        var consentimento = await _repository
            .ObterConsentimentoAtivoAsync(command.IdCliente, command.Permissao);

        if (consentimento == null)
            return ResultViewModel<RevogarConsentimentoResponse>.Error("Não existe consentimento ativo para essa permissão.");

        consentimento.Revogar();
        await _repository.AtualizarAsync(consentimento);

        var response = new RevogarConsentimentoResponse
        {
            Id = consentimento.Id,
            Permissao = consentimento.Permissao.ToString(),
            DataRevogacao = consentimento.Data
        };

        return ResultViewModel<RevogarConsentimentoResponse>.Success(response);
    }
}