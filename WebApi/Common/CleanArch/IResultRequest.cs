namespace Common.CleanArch;

/// <summary>
/// Interface for a result request.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IResultRequest<TResult> : MediatR.IRequest<Result<TResult>>
{ }