namespace Domain.Common;

/// <summary>
/// Aggregate roots are the entry points into an aggregate.
/// Only the aggregate root may be referenced by objects outside the aggregate.
/// </summary>
public abstract class AggregateRoot : Entity
{
}
