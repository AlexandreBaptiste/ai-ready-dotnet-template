namespace Application.Features.Samples.Queries;

/// <summary>
/// Handler for <see cref="GetSamplesQuery"/>.
///
/// Inject your repositories here:
///   private readonly IRepository&lt;Sample&gt; _repository;
/// </summary>
public sealed class GetSamplesQueryHandler
    : IRequestHandler<GetSamplesQuery, IReadOnlyList<SampleDto>>
{
    public async Task<IReadOnlyList<SampleDto>> Handle(
        GetSamplesQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: replace with a real repository call
        // var entities = await _repository.GetAllAsync(cancellationToken);
        // return entities.Select(e => new SampleDto(e.Id, e.Name, e.CreatedAt)).ToList();

        await Task.CompletedTask; // remove when implementing
        return [];
    }
}
