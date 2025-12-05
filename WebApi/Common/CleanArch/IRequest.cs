namespace Common.CleanArch;

/// <summary>
/// Interface for a request.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IRequest<TResult> : MediatR.IRequest<TResult>
{ }