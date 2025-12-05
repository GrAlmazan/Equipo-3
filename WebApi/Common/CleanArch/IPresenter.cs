namespace Common.CleanArch;

/// <summary>
/// Interface for a presenter.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IPresenter<TResult> : MediatR.INotificationHandler<TResult>
    where TResult : IResponse
{ }