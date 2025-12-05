namespace Common;

/// <summary>
/// Represents a result.
/// </summary>
public abstract class Result : MediatR.INotification
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> OK<T>(T data) => new SuccessResult<T>(data);
    
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> Fail<T>(string message) => new FailureResult<T>(message);
    
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    /// <param name="ex"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> Fail<T>(Exception ex) => new FailureResult<T>(ex);
}

/// <summary>
/// Represents a result.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Result<T> : MediatR.INotification
{ }