namespace Domain.Common;

/// <summary>
/// Value objects have no identity — they are equal when all their components are equal.
/// Override <see cref="GetEqualityComponents"/> to define equality.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(ValueObject? other) =>
        other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override bool Equals(object? obj) =>
        obj is ValueObject other && Equals(other);

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Aggregate(0, (current, obj) => HashCode.Combine(current, obj?.GetHashCode() ?? 0));

    public static bool operator ==(ValueObject left, ValueObject right) => left.Equals(right);
    public static bool operator !=(ValueObject left, ValueObject right) => !left.Equals(right);
}
