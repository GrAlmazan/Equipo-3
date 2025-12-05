namespace Common;

/// <summary>
/// Represents a list of error messages.
/// </summary>
public sealed class ErrorList : List<string>
{
    /// <summary>
    /// Creates a new instance of the <see cref="ErrorList"/> class with the specified error messages.
    /// </summary>
    public bool IsEmpty => this.Count == 0;

    /// <summary>
    /// Creates a new instance of the <see cref="ErrorList"/> class with the specified error messages.
    /// </summary>
    /// <returns></returns>
    public BusinessRuleException AsException() => new(ToString());

    /// <summary>
    /// Creates a new instance of the <see cref="ErrorList"/> class with the specified error messages.
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        this.Select(item => $"- {item}").Aggregate((x, y) => $"{x}\n{y}");
}