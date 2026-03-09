---
name: dotnet-best-practices
description: 'Ensure .NET/C# code meets best practices for the solution/project.'
---

# .NET/C# Best Practices

Your task is to ensure .NET/C# code in `${selection}` meets the best practices specific to this solution/project. This includes:

## Documentation & Structure

- Create comprehensive XML documentation comments for all public classes, interfaces, methods, and properties
- Include parameter descriptions and return value descriptions in XML comments
- Follow the established namespace structure: `{ProjectName}.{Layer}.{Feature}`

## Design Patterns & Architecture

- Use primary constructor syntax for dependency injection (e.g., `public class MyClass(IDependency dependency)`)
- Implement the Command/Query Handler pattern with MediatR
- Use interface segregation with clear naming conventions (prefix interfaces with `I`)
- Follow the Factory pattern for complex object creation

## Dependency Injection & Services

- Use constructor dependency injection with null checks via `ArgumentNullException.ThrowIfNull()`
- Register services with appropriate lifetimes (Singleton, Scoped, Transient)
- Use `Microsoft.Extensions.DependencyInjection` patterns
- Implement service interfaces for testability

## Async/Await Patterns

- Use `async`/`await` for all I/O operations and long-running tasks
- Return `Task` or `Task<T>` from async methods
- Use `ConfigureAwait(false)` in library code
- Pass `CancellationToken` end-to-end
- Never use `.Result` or `.Wait()` — it causes deadlocks

## Testing Standards

- Use xUnit with FluentAssertions for assertions
- Follow AAA pattern (Arrange, Act, Assert) with blank lines between each
- Use NSubstitute for mocking dependencies
- Test both success and failure scenarios
- Include null parameter validation tests
- Name tests: `MethodUnderTest_StateUnderTest_ExpectedBehavior`

## Configuration & Settings

- Use strongly-typed configuration classes with data annotations
- Use `IConfiguration` binding for settings
- Support `appsettings.json` + environment-specific overrides
- Never hardcode connection strings, API keys, or secrets

## Error Handling & Logging

- Use structured logging with `Microsoft.Extensions.Logging`
- Include scoped logging with meaningful context using `ILogger<T>`
- Use `Result<T>` pattern for expected failures — don't throw for expected errors
- Use specific exception types with descriptive messages for unexpected failures
- Never log PII, passwords, or tokens

## Performance & Security

- Use C# 13+ features and .NET 10 optimizations
- Implement proper input validation and sanitization at the boundary
- Use parameterized queries for database operations
- Enable nullable reference types; treat compiler warnings as errors
- Use `Span<T>` / `Memory<T>` for hot paths

## Code Quality

- Ensure SOLID principles compliance
- Avoid code duplication through shared services and extension methods
- Use meaningful names that reflect domain concepts
- Keep methods focused and cohesive (< 20 lines ideally)
- Remove unused `using` directives and dead code
