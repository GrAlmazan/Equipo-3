namespace Common.CleanArch;

/// <summary>
/// Interface for an interactor that returns a result.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IResultInteractor<TRequest, TResult> : MediatR.IRequestHandler<TRequest, Result<TResult>>
    where TRequest : IResultRequest<TResult>
{ }