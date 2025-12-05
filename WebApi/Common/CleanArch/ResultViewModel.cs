namespace Common.CleanArch;

/// <summary>
/// ResultViewModel class.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResultViewModel<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultViewModel{T}"/> class.
    /// </summary>
    public ResultViewModel()
    {
        Data = null;
        Message = string.Empty;
        IsSuccess = false;
        UtcTimeStamp = DateTime.UtcNow;
    }

    /// <summary>
    /// The data returned in the success response.
    /// </summary>
    public object? Data { get; private set; }

    /// <summary>
    /// The error code returned in the failure response.
    /// Example: true or false
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// The error code returned in the failure response.
    /// </summary>
    public string? Message { get; private set; }

    /// <summary>
    /// The timestamp of the response in UTC.
    /// </summary>
    public DateTime UtcTimeStamp { get; private set; }

    /// <summary>
    /// Creates a new instance of the <see cref="ResultViewModel{T}"/> class with the specified data.
    /// </summary>
    /// <param name="failure"></param>
    public void Set(IFailure failure)
    {
        Data = null;
        Message = failure.Message;
        IsSuccess = false;
        UtcTimeStamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ResultViewModel{T}"/> class with the specified data.
    /// </summary>
    /// <param name="success"></param>
    /// <param name="callback"></param>
    /// <typeparam name="TData"></typeparam>
    public void Set<TData>(ISuccess<TData> success, Func<TData, object>? callback = null)
    {
        Data = callback?.Invoke(success.Data) ?? success.Data;
        IsSuccess = true;
        UtcTimeStamp = DateTime.UtcNow;
        Message = null;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ResultViewModel{T}"/> class with the specified data.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public ResultViewModel<T> Fail(string message)
    {
        Data = null;
        Message = message;
        IsSuccess = false;
        UtcTimeStamp = DateTime.UtcNow;
        return this;
    }
}