namespace Common;

/// <summary>
/// Represents a business rule exception.
/// </summary>
public class BusinessRuleException : Exception
{
    /// <summary>
    /// Creates a new instance of the <see cref="BusinessRuleException"/> class with the specified error message.
    /// </summary>
    public BusinessRuleException()
    { }

    /// <summary>
    /// Creates a new instance of the <see cref="BusinessRuleException"/> class with the specified error message.
    /// </summary>
    /// <param name="message"></param>
    public BusinessRuleException(string? message)
        : base(message)
    { }

    /// <summary>
    /// Creates a new instance of the <see cref="BusinessRuleException"/> class with the specified error message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public BusinessRuleException(string? message, Exception? innerException)
        : base(message, innerException)
    { }
}