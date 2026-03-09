namespace Application.Features.Samples.Queries;

/// <summary>
/// Example query: returns a list of sample items.
///
/// Pattern:
///   1. Add a record implementing IRequest&lt;TResponse&gt;.
///   2. Add a handler in the same folder implementing IRequestHandler&lt;TQuery, TResponse&gt;.
///   3. Add a FluentValidation validator if the query has parameters.
///   4. The MediatR pipeline automatically runs validation + logging.
/// </summary>
public sealed record GetSamplesQuery : IRequest<IReadOnlyList<SampleDto>>;

public sealed record SampleDto(Guid Id, string Name, DateTimeOffset CreatedAt);
