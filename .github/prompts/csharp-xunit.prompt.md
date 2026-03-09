---
name: csharp-xunit
description: 'Get best practices for XUnit unit testing, including data-driven tests'
---

# XUnit Best Practices

Your goal is to help me write effective unit tests with XUnit, covering both standard and data-driven testing approaches.

## Project Setup

- Use a separate test project with naming convention `[ProjectName].Tests`
- Reference `Microsoft.NET.Test.Sdk`, `xunit`, and `xunit.runner.visualstudio` packages
- Add `FluentAssertions` for readable assertions
- Add `NSubstitute` for mocking
- Create test classes that match the classes being tested (e.g., `OrderServiceTests` for `OrderService`)
- Use `dotnet test` for running tests

## Test Structure

- No test class attributes required (unlike MSTest/NUnit)
- Use `[Fact]` for simple tests with no parameters
- Use `[Theory]` + `[InlineData]` / `[MemberData]` / `[ClassData]` for parameterised tests
- Follow the **Arrange–Act–Assert (AAA)** pattern with a blank line between each section
- Name tests using: `MethodName_Scenario_ExpectedBehavior`
- Use constructor for shared setup (runs per test); implement `IDisposable` for teardown
- Use `IClassFixture<T>` for shared context between tests in a class (e.g., expensive resources)

## Standard Tests

```csharp
public class CreateOrderCommandHandlerTests
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateOrderCommandHandler _sut;

    public CreateOrderCommandHandlerTests()
    {
        _repository = Substitute.For<IOrderRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new CreateOrderCommandHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResultWithId()
    {
        // Arrange
        var command = new CreateOrderCommand("customer-1", 100.00m);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
```

## Data-Driven Tests

```csharp
[Theory]
[InlineData("", 100)]   // empty customer ID
[InlineData(null, 100)] // null customer ID
[InlineData("c-1", 0)]  // zero amount
[InlineData("c-1", -1)] // negative amount
public async Task Handle_InvalidInputs_ReturnsFailureResult(string customerId, decimal amount)
{
    // Arrange
    var command = new CreateOrderCommand(customerId, amount);

    // Act
    var result = await _sut.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeFalse();
}
```

## Assertions (FluentAssertions)

- `result.Should().Be(expected)` — value equality
- `result.Should().BeNull()` / `.NotBeNull()`
- `collection.Should().HaveCount(3)`
- `collection.Should().Contain(item)`
- `action.Should().Throw<ArgumentNullException>()`
- `await asyncAction.Should().ThrowAsync<InvalidOperationException>()`
- `obj.Should().BeEquivalentTo(expected)` — deep structural equality

## Mocking with NSubstitute

```csharp
// Create substitute
var repository = Substitute.For<IOrderRepository>();

// Setup return value
repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
    .Returns(order);

// Verify call was made
await repository.Received(1).AddAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
```

## Test Organization

- Group tests by feature or component (one test class per production class)
- Use `[Trait("Category", "Unit")]` for categorization
- Use `ITestOutputHelper` for diagnostic output instead of `Console.WriteLine`
- Mark slow tests with `[Trait("Category", "Slow")]` to exclude from fast-feedback runs
