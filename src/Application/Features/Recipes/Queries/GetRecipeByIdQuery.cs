using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Recipes.Queries;
using Domain.Categories;
using Domain.Recipes;

namespace Application.Features.Recipes.Queries;

// ── Query ─────────────────────────────────────────────────────────────────────

/// <summary>Returns a single recipe by its unique identifier.</summary>
/// <param name="Id">The recipe identifier.</param>
public sealed record GetRecipeByIdQuery(Guid Id) : IRequest<Result<RecipeDto>>;

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="GetRecipeByIdQuery"/>.</summary>
public sealed class GetRecipeByIdQueryHandler(
    IRepository<Recipe> recipeRepository,
    IRepository<Category> categoryRepository)
    : IRequestHandler<GetRecipeByIdQuery, Result<RecipeDto>>
{
    /// <inheritdoc/>
    public async Task<Result<RecipeDto>> Handle(
        GetRecipeByIdQuery request,
        CancellationToken cancellationToken)
    {
        Recipe? recipe = await recipeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (recipe is null)
            return Result<RecipeDto>.Failure($"Recipe '{request.Id}' not found.");

        Category? category = await categoryRepository.GetByIdAsync(recipe.CategoryId, cancellationToken);

        RecipeDto dto = new(
            recipe.Id,
            recipe.Name,
            recipe.Description,
            recipe.Instructions,
            recipe.PrepTimeMinutes,
            recipe.Difficulty,
            recipe.CategoryId,
            category?.Name ?? string.Empty,
            recipe.Ingredients
                .Select(i => new IngredientDto(i.Id, i.Name, i.Quantity, i.Unit))
                .ToList());

        return Result<RecipeDto>.Success(dto);
    }
}
