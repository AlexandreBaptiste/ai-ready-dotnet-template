using System.Diagnostics;

namespace Application.Common.Behaviours;

/// <summary>
/// MediatR pipeline behavior that logs every request with its execution time.
/// Long-running requests (>500 ms) are logged as warnings.
/// </summary>
public sealed class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling {RequestName}", requestName);

        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (sw.ElapsedMilliseconds > SlowRequestThresholdMs)
            logger.LogWarning(
                "Slow request detected: {RequestName} took {ElapsedMs} ms",
                requestName, sw.ElapsedMilliseconds);
        else
            logger.LogInformation(
                "Handled {RequestName} in {ElapsedMs} ms",
                requestName, sw.ElapsedMilliseconds);

        return response;
    }
}
