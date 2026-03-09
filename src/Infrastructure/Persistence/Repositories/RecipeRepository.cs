using Application.Common.Interfaces;
using Domain.Recipes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Recipe-specific repository that eager-loads the <see cref="Recipe.Ingredients"/> collection.
/// </summary>
public sealed class RecipeRepository(AppDbContext dbContext)
    : GenericRepository<Recipe>(dbContext), IRepository<Recipe>
{
    public override async Task<IReadOnlyList<Recipe>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking()
            .Include(r => r.Ingredients)
            .ToListAsync(cancellationToken);

    public override async Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking()
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
}
