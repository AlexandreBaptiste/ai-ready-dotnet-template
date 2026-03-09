using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Categories;
using FluentValidation;

namespace Application.Features.Categories.Commands;

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>Creates a new recipe category and returns its identifier.</summary>
/// <param name="Name">Non-empty category name.</param>
public sealed record CreateCategoryCommand(string Name) : IRequest<Result<Guid>>;

// ── Validator ─────────────────────────────────────────────────────────────────

/// <summary>Validates <see cref="CreateCategoryCommand"/>.</summary>
public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="CreateCategoryCommand"/>.</summary>
public sealed class CreateCategoryCommandHandler(
    IRepository<Category> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    /// <inheritdoc/>
    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        Category category = Category.Create(request.Name);

        await repository.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);
    }
}
