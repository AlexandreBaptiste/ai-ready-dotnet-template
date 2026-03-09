using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core database context.
///
/// Usage:
///   - Add <c>DbSet&lt;YourEntity&gt;</c> properties here.
///   - Configure entity mappings in <c>OnModelCreating</c> or in separate
///     <c>IEntityTypeConfiguration&lt;T&gt;</c> classes inside the Configurations/ folder.
///   - Run <c>dotnet ef migrations add MigrationName -p src/Infrastructure -s src/Api</c>
///     to generate migrations.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    // ── Add your DbSets here ──────────────────────────────────────────────────
    // Example: public DbSet<Order> Orders => Set<Order>();

    // ─────────────────────────────────────────────────────────────────────────

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto-apply all IEntityTypeConfiguration<T> classes in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
