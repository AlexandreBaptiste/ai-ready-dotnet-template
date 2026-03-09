using Application.Common.Interfaces;
using Domain.Recipes;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Infrastructure layer DI registration.
/// Call from the composition root (Program.cs): services.AddInfrastructure(configuration);
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── EF Core ───────────────────────────────────────────────────────────
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // ── Repositories ──────────────────────────────────────────────────────
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRepository<Recipe>, RecipeRepository>();

        // IUnitOfWork is implemented by AppDbContext — resolve it from the same scope
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }
}
