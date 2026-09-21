using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Clientes.AnonimizarCliente;

public class AnonimizarClienteHandler(IClienteRepository _clienteRepository, IUsuarioRepository _usuarioRepository) : IRequestHandler<AnonimizarClienteCommand, ResultViewModel<bool>>
{
    public async Task<ResultViewModel<bool>> Handle(AnonimizarClienteCommand command, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.ObterPorIdComUsuarioAsync(command.Id);

        if (cliente == null)
            return ResultViewModel<bool>.Error("Cliente não encontrado.");

        cliente.Anonimizar();
        await _clienteRepository.AtualizarAsync(cliente);

        if (cliente.Usuario != null)
        {
            cliente.Usuario.AtualizarEmail($"anonimizado_{cliente.Id}@removed.com");
            cliente.Usuario.AtualizarSenha("CONTA_ANONIMIZADA");
            await _usuarioRepository.AtualizarAsync(cliente.Usuario);
        }

        return ResultViewModel<bool>.Success(true, "Dados anonimizados com sucesso. Os pedidos permanecem para auditoria.");
    }
}
