using FluentValidation;
using MediatR;

namespace Common.CleanArch;

/// <summary>
/// Pipeline que ejecuta validaciones automáticas antes del Interactor.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 1. Si no hay validadores para este comando, sigue adelante.
        if (!validators.Any()) return await next();

        // 2. Ejecutar todas las validaciones encontradas
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        // 3. Recolectar errores
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            // 4. Si hay errores, lanzar Excepción (Tu InteractorPipeline la atrapará y logueará)
            var errors = string.Join("\n- ", failures.Select(f => f.ErrorMessage));
            throw new BusinessRuleException($"Errores de validación:\n- {errors}");
        }

        return await next();
    }
}