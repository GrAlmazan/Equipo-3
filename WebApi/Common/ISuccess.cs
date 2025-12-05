namespace Common;

/// <summary>
/// Interface for success responses.
/// </summary>
public interface ISuccess
{ }

/// <summary>
/// Interface for success responses with data.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISuccess<T> : ISuccess
{
    /// <summary>
    /// The data returned in the success response.
    /// </summary>
    T Data { get; }
}