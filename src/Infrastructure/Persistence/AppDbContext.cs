using Application.Common.Interfaces;
using Domain.Categories;
using Domain.Recipes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the Pastry Recipes application.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    /// <summary>Gets the Recipes table.</summary>
    public DbSet<Recipe> Recipes => Set<Recipe>();

    /// <summary>Gets the Categories table.</summary>
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto-apply all IEntityTypeConfiguration<T> classes in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
