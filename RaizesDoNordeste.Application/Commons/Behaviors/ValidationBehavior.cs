using FluentValidation;
using MediatR;

namespace RaizesDoNordeste.Application.Commons.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count > 0)
        {
            var erros = string.Join("; ", failures.Select(e => e.ErrorMessage));

            var resultType = typeof(TResponse);
            var errorMethod = resultType.GetMethod("Error", new[] { typeof(string) });

            if (errorMethod != null)
                return (TResponse)errorMethod.Invoke(null, new object[] { erros })!;

            throw new ValidationException(failures);
        }

        return await next();
    }
}
