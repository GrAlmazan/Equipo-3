namespace Common.Infra.HttpApi;

/// <summary>
/// Represents a JSON response from an HTTP API.
/// </summary>
/// <param name="IsSuccess"></param>
/// <param name="Message"></param>
/// <param name="UtcTimeStamp"></param>
public record HttpApiJsonResponse(bool IsSuccess, string? Message, DateTime UtcTimeStamp);

/// <summary>
/// Represents a JSON response from an HTTP API with data.
/// </summary>
/// <param name="IsSuccess"></param>
/// <param name="Message"></param>
/// <param name="Data"></param>
/// <param name="UtcTimeStamp"></param>
/// <typeparam name="TData"></typeparam>
public record HttpApiJsonResponse<TData>(bool IsSuccess, string? Message, TData? Data, DateTime UtcTimeStamp) : HttpApiJsonResponse(IsSuccess, Message, UtcTimeStamp);