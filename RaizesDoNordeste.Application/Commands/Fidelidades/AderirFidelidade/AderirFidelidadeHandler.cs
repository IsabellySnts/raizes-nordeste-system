using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.AderirFidelidade;

public class AderirFidelidadeHandler(IFidelidadeRepository _fidelidadeRepository,IClienteRepository _clienteRepository) : IRequestHandler<AderirFidelidadeCommand, ResultViewModel<AderirFidelidadeResponse>>
{
    public async Task<ResultViewModel<AderirFidelidadeResponse>> Handle(
        AderirFidelidadeCommand command, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(command.IdCliente);
        if (cliente == null)
            return ResultViewModel<AderirFidelidadeResponse>.Error("Cliente não encontrado.");

        var jaAderiu = await _fidelidadeRepository.ClienteJaPossuiFidelidadeAsync(command.IdCliente);
        if (jaAderiu)
            return ResultViewModel<AderirFidelidadeResponse>.Error("Cliente já está cadastrado no programa de fidelidade.");

        var fidelidade = new Fidelidade(command.IdCliente);
        var fidelidadeCriada = await _fidelidadeRepository.CriarAsync(fidelidade);

        var response = new AderirFidelidadeResponse
        {
            Id = fidelidadeCriada.Id,
            IdCliente = fidelidadeCriada.IdCliente,
            NomeCliente = cliente.NomeCompleto,
            SaldoPontos = fidelidadeCriada.SaldoPontos,
            Nivel = fidelidadeCriada.Nivel.ToString()
        };

        return ResultViewModel<AderirFidelidadeResponse>.Success(response, "Bem-vindo ao programa de fidelidade!");
    }
}