using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Consentimento.RegistrarConsentimento;

public class RegistrarConsentimentoHandler(
    IConsentimentoLGPDRepository _consentimentoRepository,
    IClienteRepository _clienteRepository)
    : IRequestHandler<RegistrarConsentimentoCommand, ResultViewModel<RegistrarConsentimentoResponse>>
{
    public async Task<ResultViewModel<RegistrarConsentimentoResponse>> Handle(
        RegistrarConsentimentoCommand command, CancellationToken cancellationToken)
    {
        var validator = new RegistrarConsentimentoCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<RegistrarConsentimentoResponse>.Error(erros);
        }

        var cliente = await _clienteRepository.ObterPorIdAsync(command.IdCliente);
        if (cliente == null)
            return ResultViewModel<RegistrarConsentimentoResponse>.Error("Cliente não encontrado.");

        var consentimentoAtivo = await _consentimentoRepository
            .ObterConsentimentoAtivoAsync(command.IdCliente, command.Permissao);

        if (consentimentoAtivo != null)
        {
            consentimentoAtivo.Revogar();
            await _consentimentoRepository.AtualizarAsync(consentimentoAtivo);
        }

        var consentimento = new ConsentimentoLGPD(
            command.IdCliente,
            command.Permissao,
            command.Aceite
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
