using Domain.Common;

namespace Domain.Categories;

/// <summary>
/// Category aggregate root — groups pastry recipes by type (e.g. "Cakes", "Cookies").
/// </summary>
public sealed class Category : AggregateRoot
{
    /// <summary>Gets the display name of the category.</summary>
    public string Name { get; private set; } = default!;

    private Category() { } // required by EF Core

    /// <summary>Creates and returns a new <see cref="Category"/>.</summary>
    /// <param name="name">Non-empty category name.</param>
    public static Category Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Category { Name = name.Trim() };
    }

    /// <summary>Renames the category.</summary>
    /// <param name="name">Non-empty new name.</param>
    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
    }
}
