namespace Common.CleanArch;

/// <summary>
/// Base class for a result interactor.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class ResultInteractorBase<TRequest, TResult> : IResultInteractor<TRequest, TResult>
    where TRequest : IResultRequest<TResult>
{
    /// <summary>
    /// Creates a result with the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected Result<TResult> OK(TResult data) => Result.OK(data);

    /// <summary>
    /// Creates a result with the specified data and message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected Result<TResult> Fail(string message) => Result.Fail<TResult>(message);

    /// <summary>
    /// Creates a result with the specified exception.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    protected Result<TResult> Fail(Exception ex) => Result.Fail<TResult>(ex);

    /// <summary>
    /// Handles the request and returns a result.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<Result<TResult>> Handle(TRequest request, CancellationToken cancellationToken);
}