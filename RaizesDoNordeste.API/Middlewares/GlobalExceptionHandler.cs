using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using RaizesDoNordeste.Application.Commons;
using RaizesDoNordeste.Domain.Enums;
using System.Net;

namespace RaizesDoNordeste.API.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message, status) = exception switch
        {
                ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)),
                ResultStatus.ValidationError),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Acesso não autorizado.",
                ResultStatus.Forbidden),

            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "Recurso não encontrado.",
                ResultStatus.NotFound),

            InvalidOperationException ex => (
                HttpStatusCode.Conflict,
                ex.Message,
                ResultStatus.ValidationError),

            _ => (
                HttpStatusCode.InternalServerError,
                "Erro interno do servidor.",
                ResultStatus.ValidationError)
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
        else
            logger.LogWarning("Exceção tratada ({StatusCode}): {Message}", (int)statusCode, exception.Message);

        httpContext.Response.StatusCode = (int)statusCode;

        var response = ResultViewModel.Error(message, status);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
