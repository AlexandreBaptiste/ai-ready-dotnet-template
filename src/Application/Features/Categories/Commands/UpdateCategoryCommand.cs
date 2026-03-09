using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Categories;
using FluentValidation;

namespace Application.Features.Categories.Commands;

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>Renames an existing category.</summary>
public sealed record UpdateCategoryCommand(Guid Id, string Name) : IRequest<Result>;

// ── Validator ─────────────────────────────────────────────────────────────────

/// <summary>Validates <see cref="UpdateCategoryCommand"/>.</summary>
public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="UpdateCategoryCommand"/>.</summary>
public sealed class UpdateCategoryCommandHandler(
    IRepository<Category> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand, Result>
{
    /// <inheritdoc/>
    public async Task<Result> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        Category? category = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure($"Category '{request.Id}' not found.");

        category.Rename(request.Name);

        repository.Update(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
