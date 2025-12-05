namespace Common;

/// <summary>
/// Interface for failure responses.
/// </summary>
public interface IFailure
{
    /// <summary>
    /// The error code returned in the failure response.
    /// </summary>
    string Message { get; }
}