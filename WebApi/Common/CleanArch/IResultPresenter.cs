namespace Common.CleanArch;

/// <summary>
/// Interface for a result presenter.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IResultPresenter<TResponse> : MediatR.INotificationHandler<Result<TResponse>>
{ }