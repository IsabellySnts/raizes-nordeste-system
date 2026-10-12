using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RevogarConsentimento;

public class RevogarConsentimentoHandler(IConsentimentoLGPDRepository _repository, IClienteRepository _clienteRepository) : IRequestHandler<RevogarConsentimentoCommand, ResultViewModel<RevogarConsentimentoResponse>>
{
    public async Task<ResultViewModel<RevogarConsentimentoResponse>> Handle(RevogarConsentimentoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(command.IdCliente);

            if (cliente == null)
                return ResultViewModel<RevogarConsentimentoResponse>.Error("Cliente não encontrado.", ResultStatus.NotFound);

            if (command.PerfilLogado == PerfilUsuario.Cliente && cliente.IdUsuario != command.IdUsuarioLogado)
                return ResultViewModel<RevogarConsentimentoResponse>.Error("Você não tem permissão para consultar consentimentos de outro cliente.", ResultStatus.Forbidden);

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
        catch(Exception ex)
        {
            return ResultViewModel<RevogarConsentimentoResponse>.Error($"Ocorreu um erro ao revogar os consentimentos do cliente: {ex.Message}", ResultStatus.InternalServerError);

        }
    }
}