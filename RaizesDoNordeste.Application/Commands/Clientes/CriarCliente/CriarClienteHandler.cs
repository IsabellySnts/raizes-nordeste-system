using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Clientes.CriarCliente;

public class CriarClienteHandler(IClienteRepository _clienteRepository, IUsuarioRepository _usuarioRepository): IRequestHandler<CriarClienteCommand, ResultViewModel<CriarClienteResponse>>
{
    public async Task<ResultViewModel<CriarClienteResponse>> Handle(CriarClienteCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarClienteValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<CriarClienteResponse>.Error(erros);
        }

        var cpfExiste = await _clienteRepository.CpfExisteAsync(command.Cpf);
        if (cpfExiste)
            return ResultViewModel<CriarClienteResponse>.Error("Este CPF já está cadastrado.");

        var emailExiste = await _usuarioRepository.EmailExisteAsync(command.Email);
        if (emailExiste)
            return ResultViewModel<CriarClienteResponse>.Error("Este email já está cadastrado.");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(command.Senha);
        var usuario = new Usuario(command.Email, senhaHash);
        var usuarioCriado = await _usuarioRepository.CriarAsync(usuario);

        var cliente = new Cliente(
            command.NomeCompleto,
            command.Cpf,
            command.Telefone,
            command.Email,
            command.DataNascimento,
            usuarioCriado.Id
        );

        var clienteCriado = await _clienteRepository.CriarAsync(cliente);

        var response = new CriarClienteResponse
        {
            Id = clienteCriado.Id,
            NomeCompleto = clienteCriado.NomeCompleto,
            Email = clienteCriado.Email,
            Telefone = clienteCriado.Telefone,
            DataNascimento = clienteCriado.DataNascimento,
            DataCadastro = clienteCriado.DataCadastro
        };

        return ResultViewModel<CriarClienteResponse>.Success(response);
    }
}
