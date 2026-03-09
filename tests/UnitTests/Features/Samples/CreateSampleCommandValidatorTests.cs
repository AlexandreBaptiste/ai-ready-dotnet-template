using Application.Features.Samples.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace UnitTests.Features.Samples;

/// <summary>
/// Unit tests for <see cref="CreateSampleCommandValidator"/>.
///
/// Convention:
///   Method name: MethodUnderTest_StateUnderTest_ExpectedBehavior
///   Use FluentAssertions for assertions and FluentValidation's TestHelper
///   for validator-specific assertions.
/// </summary>
public sealed class CreateSampleCommandValidatorTests
{
    private readonly CreateSampleCommandValidator _sut = new();

    [Fact]
    public void Validate_ValidCommand_ShouldHaveNoErrors()
    {
        var command = new CreateSampleCommand("Valid Name");
        var result = _sut.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void Validate_EmptyName_ShouldHaveValidationError(string? name)
    {
        var command = new CreateSampleCommand(name!);
        var result = _sut.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_NameExceeds200Chars_ShouldHaveValidationError()
    {
        var command = new CreateSampleCommand(new string('a', 201));
        var result = _sut.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name must not exceed 200 characters.");
    }
}
