using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Application layer DI registration.
/// Call this from the composition root (Program.cs): services.AddApplication();
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // CQRS — MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // MediatR pipeline behaviors (order matters: logging wraps validation)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // FluentValidation — auto-registers all IValidator<T> in this assembly
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
