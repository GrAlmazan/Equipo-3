using Microsoft.Extensions.Logging;

namespace Common.CleanArch;

/// <summary>
/// Pipeline behavior for handling requests and responses.
/// </summary>
/// <param name="Mediator"></param>
/// <param name="Logger"></param>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public record InteractorPipeline<TRequest, TResponse>(MediatR.IMediator Mediator, ILogger<InteractorPipeline<TRequest, TResponse>> Logger) : MediatR.IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
    where TResponse : MediatR.INotification
{
    /// <summary>
    /// Handles the request and response.
    /// </summary>
    private readonly Type _requestType = typeof(TRequest);
    
    /// <summary>
    /// Handles the request and response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, MediatR.RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response;
        Logger.LogInformation("{@Request}", request);
        try
        {
            response = await next().ConfigureAwait(false);
            if (response is IFailure failure)
            {
                var typeOfResponse = typeof(TResponse);
                var genericArgs = typeOfResponse.GetGenericArguments();
                var responseTypeName = genericArgs.Length > 0 ? genericArgs[0].Name : typeOfResponse.Name;
                Logger.LogWarning("{ResponseType}: {FailureMessage}", responseTypeName, failure.Message);
            }
            else
            {
                //object? data = ((dynamic)response).Data;
                Logger.LogInformation("{@Response}", response);
            }
            await Mediator.Publish(response).ConfigureAwait(false);
        }
        catch (BusinessRuleException ex)
        {
            Logger.LogError(ex, "Error: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            var innerEx = ex;
            while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
            Logger.LogCritical(ex, "Error crítico: {ErrorMessage}", innerEx.Message);
            throw;
        }
        return response;
    }
}