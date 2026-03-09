using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Categories;

namespace Application.Features.Categories.Commands;

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>Deletes a category by its identifier.</summary>
/// <param name="Id">The identifier of the category to delete.</param>
public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Result>;

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="DeleteCategoryCommand"/>.</summary>
public sealed class DeleteCategoryCommandHandler(
    IRepository<Category> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    /// <inheritdoc/>
    public async Task<Result> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        Category? category = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure($"Category '{request.Id}' not found.");

        repository.Remove(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
