# GitHub Copilot – Workspace Instructions

## Project Overview
This is a .NET 10 Web API using **Clean Architecture**. Every feature flows through four layers:

```
Domain  ←  Application  ←  Infrastructure  ←  Api
```

- **Domain** (`src/Domain`): entities, value objects, aggregates, domain events. Zero external dependencies.
- **Application** (`src/Application`): use cases as CQRS commands/queries via MediatR. Abstractions (interfaces) only — no EF, no HTTP.
- **Infrastructure** (`src/Infrastructure`): EF Core + SQL Server, external services. Implements Application interfaces.
- **Api** (`src/Api`): ASP.NET Core minimal API, DI composition root. Thin — just routing and DI.

---

## Architecture Rules (always enforce these)

1. **Dependency direction is strict**: Domain knows nothing. Application knows only Domain. Infrastructure knows Application + Domain. Api knows everything.
2. **No `new` keyword for services** in command/query handlers — always inject via constructor.
3. **Commands return `Result<T>`**, not raw values or exceptions for expected failures.
4. **Every command must have a FluentValidation validator** in the same file.
5. **Do not put business logic in controllers/endpoints** — only call `mediator.Send()`.
6. **Repositories are generic** (`IRepository<T>`) — domain-specific query methods go in typed repository subclasses.
7. **Domain entities raise domain events** — never publish them directly from handlers.

---

## Code Conventions

### Naming
| Element | Convention | Example |
|---|---|---|
| Namespace | Match folder | `Application.Features.Orders.Commands` |
| Command | Verb + Noun + `Command` | `CreateOrderCommand` |
| Query | Verb + Noun + `Query` | `GetOrderByIdQuery` |
| Handler | Same name + `Handler` suffix | `CreateOrderCommandHandler` |
| DTO | Noun + `Dto` | `OrderDto` |
| Validator | Same name as command + `Validator` | `CreateOrderCommandValidator` |
| Endpoint class | Noun + `Endpoints` | `OrdersEndpoints` |

### File Structure for a New Feature

```
src/Application/Features/{Feature}/
  Commands/
    Create{Feature}Command.cs       ← record + validator + handler
    Update{Feature}Command.cs
    Delete{Feature}Command.cs
  Queries/
    Get{Feature}sQuery.cs           ← record + dto
    Get{Feature}sQueryHandler.cs    ← handler
    Get{Feature}ByIdQuery.cs
    Get{Feature}ByIdQueryHandler.cs

src/Domain/{Feature}/
  {Feature}.cs                      ← aggregate root
  {Feature}CreatedEvent.cs          ← domain event (if needed)
  Value objects inline or separate

src/Infrastructure/Persistence/
  Configurations/{Feature}Configuration.cs   ← IEntityTypeConfiguration<T>
  Repositories/{Feature}Repository.cs        ← optional typed repo

src/Api/Endpoints/
  {Feature}Endpoints.cs

tests/UnitTests/Features/{Feature}/
  Create{Feature}CommandValidatorTests.cs
  Create{Feature}CommandHandlerTests.cs

tests/IntegrationTests/Endpoints/
  {Feature}EndpointsTests.cs
```

---

## Patterns to Follow

### Command with Validator and Handler (in ONE file)
```csharp
using Application.Common.Models;
using FluentValidation;

namespace Application.Features.Orders.Commands;

public sealed record CreateOrderCommand(string CustomerId, decimal Amount) : IRequest<Result<Guid>>;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

public sealed class CreateOrderCommandHandler(
    IRepository<Order> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = Order.Create(request.CustomerId, request.Amount);
        await repository.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success(order.Id);
    }
}
```

### Minimal API Endpoint
```csharp
public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders").WithOpenApi();
        group.MapPost("/", CreateOrderAsync).WithSummary("Create order").Produces<Guid>(201).ProducesValidationProblem();
        return app;
    }

    private static async Task<IResult> CreateOrderAsync(CreateOrderCommand cmd, ISender mediator, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess
            ? Results.Created($"/api/orders/{result.Value}", result.Value)
            : Results.Problem(result.Error, statusCode: 422);
    }
}
```

### Domain Entity
```csharp
public sealed class Order : AggregateRoot
{
    public string CustomerId { get; private set; } = default!;
    public decimal Amount { get; private set; }

    private Order() { }  // EF Core

    public static Order Create(string customerId, decimal amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerId);
        if (amount <= 0) throw new DomainException("Amount must be positive.");

        var order = new Order { CustomerId = customerId, Amount = amount };
        order.RaiseDomainEvent(new OrderCreatedEvent(order.Id));
        return order;
    }
}
```

---

## Testing Guidelines

- **Unit tests**: test validators and handlers in isolation. Use `NSubstitute` for mocks, `FluentAssertions` for assertions, `FluentValidation.TestHelper` for validators.
- **Integration tests**: use `AppFactory` (Testcontainers SQL Server). Test full HTTP round-trips.
- Name tests using: `MethodUnderTest_StateUnderTest_ExpectedBehavior`
- Arrange/Act/Assert pattern with blank lines between sections.

---

## What Copilot Should NOT Do

- Do not add `[ApiController]` or `ControllerBase`  — this project uses Minimal APIs.
- Do not put logic in `Program.cs` — only registration and pipeline setup.
- Do not use `var` for return types in public methods — always use explicit types.
- Do not skip validators for commands.
- Do not reference `Infrastructure` from `Application` — only interfaces.
- Do not create static helper classes with business logic — use domain services or extension methods on domain types.
