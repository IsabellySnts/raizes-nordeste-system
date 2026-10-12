using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enums;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;

public class RegistrarConsentimentoHandler(
    IConsentimentoLGPDRepository _consentimentoRepository,
    IClienteRepository _clienteRepository)
    : IRequestHandler<RegistrarConsentimentoCommand, ResultViewModel<RegistrarConsentimentoResponse>>
{
    public async Task<ResultViewModel<RegistrarConsentimentoResponse>> Handle(RegistrarConsentimentoCommand command, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(command.IdCliente);

        if (cliente == null)
            return ResultViewModel<RegistrarConsentimentoResponse>.Error("Cliente não encontrado.", ResultStatus.NotFound);

        if (command.PerfilLogado == PerfilUsuario.Cliente && cliente.IdUsuario != command.IdUsuarioLogado)
            return ResultViewModel<RegistrarConsentimentoResponse>.Error("Você não tem permissão para gerenciar consentimentos de outro cliente.", ResultStatus.Forbidden);

        var consentimentoAtivo = await _consentimentoRepository.ObterConsentimentoAtivoAsync(command.IdCliente, command.Permissao);

        if (consentimentoAtivo != null)
            return ResultViewModel<RegistrarConsentimentoResponse>.Error($"O consentimento de '{command.Permissao}' já está ativo. Use o endpoint de revogação para alterá-lo.");

        var consentimento = new ConsentimentoLGPD(
            command.IdCliente,
            command.Permissao
        );

        var criado = await _consentimentoRepository.CriarAsync(consentimento);

        var response = new RegistrarConsentimentoResponse
        {
            Id = criado.Id,
            IdCliente = criado.IdCliente,
            Permissao = criado.Permissao.ToString(),
            Aceite = criado.Aceite,
            Data = criado.Data
        };

        return ResultViewModel<RegistrarConsentimentoResponse>.Success(response);
    }
}
