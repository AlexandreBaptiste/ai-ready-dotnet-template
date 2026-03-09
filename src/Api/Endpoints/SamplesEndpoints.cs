using Application.Features.Samples.Commands;
using Application.Features.Samples.Queries;
using MediatR;

namespace Api.Endpoints;

/// <summary>
/// Minimal API endpoints for the Samples feature.
///
/// Convention:
///   - One static class per feature with a MapXxxEndpoints extension method.
///   - Register all groups in Program.cs: app.MapSamplesEndpoints();
///   - Use WithTags, WithSummary, and Produces for OpenAPI documentation.
/// </summary>
public static class SamplesEndpoints
{
    public static IEndpointRouteBuilder MapSamplesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/samples")
                       .WithTags("Samples")
                       .WithOpenApi();

        group.MapGet("/", GetSamplesAsync)
             .WithSummary("Get all samples")
             .Produces<IReadOnlyList<SampleDto>>();

        group.MapPost("/", CreateSampleAsync)
             .WithSummary("Create a new sample")
             .Produces<Guid>(StatusCodes.Status201Created)
             .ProducesValidationProblem();

        return app;
    }

    // ── Handlers ──────────────────────────────────────────────────────────────

    private static async Task<IResult> GetSamplesAsync(ISender mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSamplesQuery(), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateSampleAsync(
        CreateSampleCommand command,
        ISender mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.IsSuccess
            ? Results.Created($"/api/samples/{result.Value}", result.Value)
            : Results.Problem(result.Error, statusCode: StatusCodes.Status422UnprocessableEntity);
    }
}
