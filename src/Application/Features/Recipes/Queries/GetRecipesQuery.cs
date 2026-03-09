using Application.Common.Interfaces;
using Domain.Categories;
using Domain.Recipes;

namespace Application.Features.Recipes.Queries;

// ── DTOs ──────────────────────────────────────────────────────────────────────

/// <summary>Read model for an ingredient.</summary>
public sealed record IngredientDto(Guid Id, string Name, decimal Quantity, string Unit);

/// <summary>Read model for a recipe summary / detail.</summary>
public sealed record RecipeDto(
    Guid Id,
    string Name,
    string Description,
    string Instructions,
    int PrepTimeMinutes,
    Difficulty Difficulty,
    Guid CategoryId,
    string CategoryName,
    IReadOnlyList<IngredientDto> Ingredients);

// ── Query ─────────────────────────────────────────────────────────────────────

/// <summary>Returns all recipes.</summary>
public sealed record GetRecipesQuery : IRequest<IReadOnlyList<RecipeDto>>;

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="GetRecipesQuery"/>.</summary>
public sealed class GetRecipesQueryHandler(
    IRepository<Recipe> recipeRepository,
    IRepository<Category> categoryRepository)
    : IRequestHandler<GetRecipesQuery, IReadOnlyList<RecipeDto>>
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<RecipeDto>> Handle(
        GetRecipesQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Recipe> recipes = await recipeRepository.GetAllAsync(cancellationToken);
        IReadOnlyList<Category> categories = await categoryRepository.GetAllAsync(cancellationToken);

        var categoryNames = categories.ToDictionary(c => c.Id, c => c.Name);

        return recipes
            .Select(r => new RecipeDto(
                r.Id,
                r.Name,
                r.Description,
                r.Instructions,
                r.PrepTimeMinutes,
                r.Difficulty,
                r.CategoryId,
                categoryNames.GetValueOrDefault(r.CategoryId, string.Empty),
                r.Ingredients
                    .Select(i => new IngredientDto(i.Id, i.Name, i.Quantity, i.Unit))
                    .ToList()))
            .ToList();
    }
}
