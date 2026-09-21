using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Clientes.AtualizarCliente;

public class AtualizarClienteHandler(IClienteRepository _clienteRepository)
    : IRequestHandler<AtualizarClienteCommand, ResultViewModel<AtualizarClienteResponse>>
{
    public async Task<ResultViewModel<AtualizarClienteResponse>> Handle(
        AtualizarClienteCommand command, CancellationToken cancellationToken)
    {
        var validator = new AtualizarClienteValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AtualizarClienteResponse>.Error(erros);
        }

        var cliente = await _clienteRepository.ObterPorIdAsync(command.Id);
        if (cliente == null)
            return ResultViewModel<AtualizarClienteResponse>.Error("Cliente não encontrado.");

        var emailExiste = await _clienteRepository.EmailExisteAsync(command.Email, command.Id);
        if (emailExiste)
            return ResultViewModel<AtualizarClienteResponse>.Error("Este email já está em uso por outro cliente.");

        cliente.Atualizar(command.NomeCompleto, command.Telefone, command.Email, command.DataNascimento);

        await _clienteRepository.AtualizarAsync(cliente);

        var response = new AtualizarClienteResponse
        {
            Id = cliente.Id,
            NomeCompleto = cliente.NomeCompleto,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            DataNascimento = cliente.DataNascimento,
            DataAtualizacao = cliente.DataAtualizacao
        };

        return ResultViewModel<AtualizarClienteResponse>.Success(response);
    }
}
