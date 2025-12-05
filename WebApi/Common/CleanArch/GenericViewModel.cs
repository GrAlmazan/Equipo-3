namespace Common.CleanArch;

/// <summary>
/// Represents a generic view model.
/// </summary>
/// <typeparam name="T"></typeparam>
public class GenericViewModel<T>
{
    /// <summary>
    /// Creates a new instance of the <see cref="GenericViewModel{T}"/> class with default values.
    /// </summary>
    public GenericViewModel()
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
    /// Creates a new instance of the <see cref="GenericViewModel{T}"/> class with the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public GenericViewModel<T> Ok(object data)
    {
        Data = data;
        Message = null;
        IsSuccess = true;
        UtcTimeStamp = DateTime.UtcNow;
        return this;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GenericViewModel{T}"/> class with the specified error message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public GenericViewModel<T> Fail(string message)
    {
        Data = null;
        Message = message;
        IsSuccess = false;
        UtcTimeStamp = DateTime.UtcNow;
        return this;
    }
}