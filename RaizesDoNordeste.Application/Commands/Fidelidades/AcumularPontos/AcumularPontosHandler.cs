using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.AcumularPontos;

public class AcumularPontosHandler(IFidelidadeRepository _repository) : IRequestHandler<AcumularPontosCommand, ResultViewModel<AcumularPontosResponse>>
{
    public async Task<ResultViewModel<AcumularPontosResponse>> Handle(
        AcumularPontosCommand command, CancellationToken cancellationToken)
    {
        var validator = new AcumularPontosCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<AcumularPontosResponse>.Error(erros);
        }

        var fidelidade = await _repository.ObterPorClienteIdAsync(command.IdCliente);
        if (fidelidade == null)
            return ResultViewModel<AcumularPontosResponse>.Error("Cliente não está cadastrado no programa de fidelidade.");

        var saldoAnterior = fidelidade.SaldoPontos;

        var pontos = (int)Math.Floor(command.ValorPedido);

        fidelidade.AcumularPontos(pontos, command.IdPedido);
        await _repository.AtualizarAsync(fidelidade);

        var response = new AcumularPontosResponse
        {
            PontosAcumulados = pontos,
            SaldoAnterior = saldoAnterior,
            SaldoAtual = fidelidade.SaldoPontos,
            Nivel = fidelidade.Nivel.ToString()
        };

        return ResultViewModel<AcumularPontosResponse>.Success(response);
    }
}
