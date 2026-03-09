using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Recipes;
using FluentValidation;

namespace Application.Features.Recipes.Commands;

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>Updates an existing pastry recipe's fields (except ingredients).</summary>
public sealed record UpdateRecipeCommand(
    Guid Id,
    string Name,
    string Description,
    string Instructions,
    int PrepTimeMinutes,
    Difficulty Difficulty) : IRequest<Result>;

// ── Validator ─────────────────────────────────────────────────────────────────

/// <summary>Validates <see cref="UpdateRecipeCommand"/>.</summary>
public sealed class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
{
    public UpdateRecipeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("Instructions are required.")
            .MaximumLength(5000).WithMessage("Instructions must not exceed 5000 characters.");

        RuleFor(x => x.PrepTimeMinutes)
            .GreaterThan(0).WithMessage("Prep time must be greater than zero.");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Difficulty must be a valid value (Easy, Medium, Hard).");
    }
}

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="UpdateRecipeCommand"/>.</summary>
public sealed class UpdateRecipeCommandHandler(
    IRepository<Recipe> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRecipeCommand, Result>
{
    /// <inheritdoc/>
    public async Task<Result> Handle(
        UpdateRecipeCommand request,
        CancellationToken cancellationToken)
    {
        Recipe? recipe = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (recipe is null)
            return Result.Failure($"Recipe '{request.Id}' not found.");

        recipe.Update(
            request.Name,
            request.Description,
            request.Instructions,
            request.PrepTimeMinutes,
            request.Difficulty);

        repository.Update(recipe);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
