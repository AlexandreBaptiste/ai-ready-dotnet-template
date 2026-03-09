using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Recipes;
using FluentValidation;

namespace Application.Features.Recipes.Commands;

// ── Sub-DTO ───────────────────────────────────────────────────────────────────

/// <summary>Ingredient data supplied when creating a recipe.</summary>
public sealed record CreateIngredientDto(string Name, decimal Quantity, string Unit);

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>Creates a new pastry recipe and returns its new identifier.</summary>
public sealed record CreateRecipeCommand(
    string Name,
    string Description,
    string Instructions,
    int PrepTimeMinutes,
    Difficulty Difficulty,
    Guid CategoryId,
    IReadOnlyList<CreateIngredientDto> Ingredients) : IRequest<Result<Guid>>;

// ── Validator ─────────────────────────────────────────────────────────────────

/// <summary>Validates <see cref="CreateRecipeCommand"/>.</summary>
public sealed class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    public CreateRecipeCommandValidator()
    {
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

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleForEach(x => x.Ingredients)
            .ChildRules(i =>
            {
                i.RuleFor(x => x.Name).NotEmpty().WithMessage("Ingredient name is required.");
                i.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Ingredient quantity must be greater than zero.");
                i.RuleFor(x => x.Unit).NotEmpty().WithMessage("Ingredient unit is required.");
            });
    }
}

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="CreateRecipeCommand"/>.</summary>
public sealed class CreateRecipeCommandHandler(
    IRepository<Recipe> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRecipeCommand, Result<Guid>>
{
    /// <inheritdoc/>
    public async Task<Result<Guid>> Handle(
        CreateRecipeCommand request,
        CancellationToken cancellationToken)
    {
        Recipe recipe = Recipe.Create(
            request.Name,
            request.Description,
            request.Instructions,
            request.PrepTimeMinutes,
            request.Difficulty,
            request.CategoryId);

        foreach (CreateIngredientDto ingredient in request.Ingredients)
            recipe.AddIngredient(ingredient.Name, ingredient.Quantity, ingredient.Unit);

        await repository.AddAsync(recipe, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(recipe.Id);
    }
}
