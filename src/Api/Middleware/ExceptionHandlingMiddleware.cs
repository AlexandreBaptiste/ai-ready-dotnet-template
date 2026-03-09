using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

/// <summary>
/// Global exception handler.
/// Converts well-known exceptions into Problem Details (RFC 9457) responses.
/// All unhandled exceptions become a 500 without leaking stack traces.
/// </summary>
public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error");
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest,
                "Validation failed",
                extensions: new Dictionary<string, object?>
                {
                    ["errors"] = ex.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray())
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        int statusCode,
        string detail,
        Dictionary<string, object?>? extensions = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrasesHelper.GetReasonPhrase(statusCode),
            Detail = detail,
        };

        if (extensions is not null)
            foreach (var (key, value) in extensions)
                problem.Extensions[key] = value;

        await context.Response.WriteAsJsonAsync(problem);
    }
}

/// <summary>Maps HTTP status codes to RFC 9457 reason phrases.</summary>
file static class ReasonPhrasesHelper
{
    public static string GetReasonPhrase(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        422 => "Unprocessable Entity",
        500 => "Internal Server Error",
        _ => "Error"
    };
}
