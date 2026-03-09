namespace Domain.Common;

/// <summary>
/// Marker interface for domain events.
/// Domain events represent something meaningful that occurred in the domain.
///
/// The Application layer bridges these to MediatR INotification so handlers
/// can subscribe without the Domain layer depending on MediatR.
///
/// Dispatch pattern:
///   After SaveChangesAsync, intercept entities that have domain events,
///   clear them, and publish each event via IMediator.Publish().
/// </summary>
public interface IDomainEvent
{
    DateTimeOffset OccurredOn => DateTimeOffset.UtcNow;
}
