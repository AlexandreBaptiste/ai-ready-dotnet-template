using Application.Common.Interfaces;
using Domain.Categories;

namespace Application.Features.Categories.Queries;

// ── DTO ───────────────────────────────────────────────────────────────────────

/// <summary>Read model for a category.</summary>
public sealed record CategoryDto(Guid Id, string Name);

// ── Query ─────────────────────────────────────────────────────────────────────

/// <summary>Returns all recipe categories.</summary>
public sealed record GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;

// ── Handler ───────────────────────────────────────────────────────────────────

/// <summary>Handles <see cref="GetCategoriesQuery"/>.</summary>
public sealed class GetCategoriesQueryHandler(IRepository<Category> repository)
    : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<CategoryDto>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Category> categories = await repository.GetAllAsync(cancellationToken);

        return categories
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToList();
    }
}
