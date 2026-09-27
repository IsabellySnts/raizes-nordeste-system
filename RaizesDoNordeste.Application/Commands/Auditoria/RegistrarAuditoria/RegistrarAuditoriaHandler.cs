using MediatR;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Application.Commands.Auditoria.RegistrarAuditoria;

public class RegistrarAuditoriaHandler(
    IAuditoriaRepository _auditoriaRepository,
    IFuncionarioRepository _funcionarioRepository)
    : IRequestHandler<RegistrarAuditoriaCommand, ResultViewModel<RegistrarAuditoriaResponse>>
{
    public async Task<ResultViewModel<RegistrarAuditoriaResponse>> Handle(
        RegistrarAuditoriaCommand command, CancellationToken cancellationToken)
    {
        var validator = new RegistrarAuditoriaCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return ResultViewModel<RegistrarAuditoriaResponse>.Error(erros);
        }

        var funcionario = await _funcionarioRepository.ObterPorIdAsync(command.IdFuncionario);
        if (funcionario == null)
            return ResultViewModel<RegistrarAuditoriaResponse>.Error("Funcionário não encontrado.");

        var auditoria = new Domain.Entities.Auditoria(
            command.IdFuncionario,
            command.Acao,
            command.TipoEntidadeAfetada,
            command.IdEntidadeAfetada,
            command.Detalhes
        );

        var auditoriaCriada = await _auditoriaRepository.CriarAsync(auditoria);

        var response = new RegistrarAuditoriaResponse
        {
            Id = auditoriaCriada.Id,
            NomeFuncionario = funcionario.Nome,
            Acao = auditoriaCriada.Acao.ToString(),
            TipoEntidadeAfetada = auditoriaCriada.TipoEntidadeAfetada,
            IdEntidadeAfetada = auditoriaCriada.IdEntidadeAfetada,
            Detalhes = auditoriaCriada.Detalhes,
            DataHora = auditoriaCriada.DataHora
        };

        return ResultViewModel<RegistrarAuditoriaResponse>.Success(response);
    }
}
