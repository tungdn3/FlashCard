using FlashCard.Api.Models;
using FlashCard.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace FlashCard.Api;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred.");

        var errorResponse = new ErrorResponse
        {
            Message = exception.Message,
        };

        switch (exception)
        {
            case FluentValidation.ValidationException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Code = "BadArgument";
                errorResponse.Details = ((FluentValidation.ValidationException)exception).Errors.Select(x => new ErrorResponse
                {
                    Code = x.ErrorCode,
                    Message = x.ErrorMessage,
                    Target = x.PropertyName,
                });
                break;

            case NotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Code = "NotFound";
                break;

            case NotAuthenticatedException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case ForbiddenException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                break;

            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}
