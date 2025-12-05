namespace Common;

/// <summary>
/// Represents a successful result.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class SuccessResult<T> : Result<T>, ISuccess<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SuccessResult{T}"/> class.
    /// </summary>
    /// <param name="data"></param>
    public SuccessResult(T data) => Data = data;

    /// <summary>
    /// Gets the data of the result.
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// Implicitly converts a <see cref="T"/> to a <see cref="SuccessResult{T}"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static implicit operator SuccessResult<T>(T data) => new(data);
}