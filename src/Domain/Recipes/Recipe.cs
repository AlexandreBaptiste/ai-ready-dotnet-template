using Domain.Common;

namespace Domain.Recipes;

/// <summary>
/// Recipe aggregate root. Encapsulates a pastry recipe with its ingredients.
/// All mutations go through the public methods of this class.
/// </summary>
public sealed class Recipe : AggregateRoot
{
    /// <summary>Gets the display name of the recipe.</summary>
    public string Name { get; private set; } = default!;

    /// <summary>Gets a short description of the recipe.</summary>
    public string Description { get; private set; } = default!;

    /// <summary>Gets the step-by-step preparation instructions.</summary>
    public string Instructions { get; private set; } = default!;

    /// <summary>Gets the estimated preparation time in minutes.</summary>
    public int PrepTimeMinutes { get; private set; }

    /// <summary>Gets the difficulty level of the recipe.</summary>
    public Difficulty Difficulty { get; private set; }

    /// <summary>Gets the identifier of the category this recipe belongs to.</summary>
    public Guid CategoryId { get; private set; }

    private readonly List<Ingredient> _ingredients = [];

    /// <summary>Gets the ingredient list for this recipe (read-only snapshot).</summary>
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients.AsReadOnly();

    private Recipe() { } // required by EF Core

    /// <summary>
    /// Creates and returns a new <see cref="Recipe"/> aggregate.
    /// </summary>
    public static Recipe Create(
        string name,
        string description,
        string instructions,
        int prepTimeMinutes,
        Difficulty difficulty,
        Guid categoryId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(instructions);

        if (prepTimeMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(prepTimeMinutes), "Prep time must be greater than zero.");

        if (categoryId == Guid.Empty)
            throw new ArgumentException("CategoryId must not be empty.", nameof(categoryId));

        return new Recipe
        {
            Name = name.Trim(),
            Description = description.Trim(),
            Instructions = instructions.Trim(),
            PrepTimeMinutes = prepTimeMinutes,
            Difficulty = difficulty,
            CategoryId = categoryId
        };
    }

    /// <summary>Updates the recipe's mutable fields.</summary>
    public void Update(
        string name,
        string description,
        string instructions,
        int prepTimeMinutes,
        Difficulty difficulty)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(instructions);

        if (prepTimeMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(prepTimeMinutes), "Prep time must be greater than zero.");

        Name = name.Trim();
        Description = description.Trim();
        Instructions = instructions.Trim();
        PrepTimeMinutes = prepTimeMinutes;
        Difficulty = difficulty;
    }

    /// <summary>Appends a new ingredient to this recipe.</summary>
    public void AddIngredient(string name, decimal quantity, string unit)
    {
        Ingredient ingredient = Ingredient.Create(name, quantity, unit);
        _ingredients.Add(ingredient);
    }

    /// <summary>Removes an ingredient identified by <paramref name="ingredientId"/>.</summary>
    public void RemoveIngredient(Guid ingredientId)
    {
        Ingredient? ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId);
        if (ingredient is null)
            throw new DomainException($"Ingredient '{ingredientId}' not found on recipe '{Id}'.");

        _ingredients.Remove(ingredient);
    }
}
