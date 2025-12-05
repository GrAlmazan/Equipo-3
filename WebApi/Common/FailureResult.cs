namespace Common;

/// <summary>
/// Represents a failure result.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class FailureResult<T> : Result<T>, IFailure
{
    /// <summary>
    /// Creates a new instance of the <see cref="FailureResult{T}"/> class with the specified exception.
    /// </summary>
    /// <param name="message"></param>
    public FailureResult(string message) => Exception = new Exception(message);

    /// <summary>
    /// Creates a new instance of the <see cref="FailureResult{T}"/> class with the specified exception.
    /// </summary>
    /// <param name="ex"></param>
    public FailureResult(Exception ex) => Exception = ex;

    /// <summary>
    /// The exception that caused the failure.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// The error code returned in the failure response.
    /// </summary>
    public string Message => Exception.Message;
}