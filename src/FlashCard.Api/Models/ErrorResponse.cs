using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlashCard.Api.Models;

public class ErrorResponse
{
    /// <summary>
    /// Required. A human-readable code defined by our service. Not an HTTP code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Required. A human-readable representation of the error that is intended as an aid to developers only.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The target of the particular error (e.g., the name of the property in error).
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// An array of details about specific errors that led to this reported error.
    /// </summary>
    public IEnumerable<ErrorResponse>? Details { get; set; }

    /// <summary>
    /// Information that will help debug the service
    /// </summary>
    public InnerError? InnerError { get; set; }
}

public class InnerError
{
    public string? Trace { get; set; }
}
