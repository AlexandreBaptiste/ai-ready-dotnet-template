using Domain.Common;

namespace Domain.Recipes;

/// <summary>
/// Ingredient is an owned entity that belongs to a <see cref="Recipe"/> aggregate.
/// It cannot exist independently of the recipe that owns it.
/// </summary>
public sealed class Ingredient : Entity
{
    /// <summary>Gets the ingredient name (e.g. "Flour").</summary>
    public string Name { get; private set; } = default!;

    /// <summary>Gets the quantity (e.g. 250).</summary>
    public decimal Quantity { get; private set; }

    /// <summary>Gets the unit of measure (e.g. "g", "ml", "cups").</summary>
    public string Unit { get; private set; } = default!;

    private Ingredient() { } // required by EF Core

    /// <summary>Creates a new <see cref="Ingredient"/> with the supplied values.</summary>
    /// <param name="name">Non-empty ingredient name.</param>
    /// <param name="quantity">Positive quantity.</param>
    /// <param name="unit">Non-empty unit of measure.</param>
    internal static Ingredient Create(string name, decimal quantity, string unit)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(unit);

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        return new Ingredient
        {
            Name = name.Trim(),
            Quantity = quantity,
            Unit = unit.Trim()
        };
    }
}
