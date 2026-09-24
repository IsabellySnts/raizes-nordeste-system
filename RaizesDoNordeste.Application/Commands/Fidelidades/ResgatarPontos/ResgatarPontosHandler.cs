using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Fidelidades.ResgatarPontos;

public class ResgatarPontosHandler(IFidelidadeRepository _repository) : IRequestHandler<ResgatarPontosCommand, ResultViewModel<ResgatarPontosResponse>>
{
    public async Task<ResultViewModel<ResgatarPontosResponse>> Handle(
        ResgatarPontosCommand command, CancellationToken cancellationToken)
    {
        var validator = new ResgatarPontosCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<ResgatarPontosResponse>.Error(erros);
        }

        var fidelidade = await _repository.ObterPorClienteIdAsync(command.IdCliente);
        if (fidelidade == null)
            return ResultViewModel<ResgatarPontosResponse>.Error("Cliente não está cadastrado no programa de fidelidade.");

        var saldoAnterior = fidelidade.SaldoPontos;

        var resgatou = fidelidade.ResgatarPontos(command.Pontos, command.IdPedido);
        if (!resgatou)
            return ResultViewModel<ResgatarPontosResponse>.Error(
                $"Saldo insuficiente. Saldo atual: {fidelidade.SaldoPontos} pontos. Tentou resgatar: {command.Pontos} pontos.");

        await _repository.AtualizarAsync(fidelidade);

        var valorDesconto = command.Pontos * 0.10m;

        var response = new ResgatarPontosResponse
        {
            PontosResgatados = command.Pontos,
            ValorDesconto = valorDesconto,
            SaldoAnterior = saldoAnterior,
            SaldoAtual = fidelidade.SaldoPontos,
            Nivel = fidelidade.Nivel.ToString()
        };

        return ResultViewModel<ResgatarPontosResponse>.Success(response);
    }
}