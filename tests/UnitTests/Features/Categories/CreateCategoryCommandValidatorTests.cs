using Application.Features.Categories.Commands;
using FluentAssertions;
using FluentValidation.Results;

namespace UnitTests.Features.Categories;

/// <summary>Unit tests for <see cref="CreateCategoryCommandValidator"/>.</summary>
public sealed class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPassValidation()
    {
        // Arrange
        CreateCategoryCommand command = new("Cakes");

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void EmptyName_ShouldFailValidation(string name)
    {
        // Arrange
        CreateCategoryCommand command = new(name);

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Name));
    }

    [Fact]
    public void NameExceeds100Chars_ShouldFailValidation()
    {
        // Arrange
        CreateCategoryCommand command = new(new string('a', 101));

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Name));
    }

    [Fact]
    public void NameExactly100Chars_ShouldPassValidation()
    {
        // Arrange
        CreateCategoryCommand command = new(new string('a', 100));

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
