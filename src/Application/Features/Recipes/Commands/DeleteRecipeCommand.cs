using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Recipes;

namespace Application.Features.Recipes.Commands;

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>Deletes a recipe by its identifier.</summary>
/// <param name="Id">The identifier of the recipe to delete.</param>
public sealed record DeleteRecipeCommand(Guid Id) : IRequest<Result>;

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="DeleteRecipeCommand"/>.</summary>
public sealed class DeleteRecipeCommandHandler(
    IRepository<Recipe> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRecipeCommand, Result>
{
    /// <inheritdoc/>
    public async Task<Result> Handle(
        DeleteRecipeCommand request,
        CancellationToken cancellationToken)
    {
        Recipe? recipe = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (recipe is null)
            return Result.Failure($"Recipe '{request.Id}' not found.");

        repository.Remove(recipe);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
