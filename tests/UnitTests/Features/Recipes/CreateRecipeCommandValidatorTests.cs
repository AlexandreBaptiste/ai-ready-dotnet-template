using Application.Features.Recipes.Commands;
using Domain.Recipes;
using FluentAssertions;
using FluentValidation.Results;

namespace UnitTests.Features.Recipes;

/// <summary>Unit tests for <see cref="CreateRecipeCommandValidator"/>.</summary>
public sealed class CreateRecipeCommandValidatorTests
{
    private readonly CreateRecipeCommandValidator _validator = new();

    private static CreateRecipeCommand ValidCommand() => new(
        Name: "Croissant",
        Description: "Classic French pastry.",
        Instructions: "Fold butter into dough repeatedly.",
        PrepTimeMinutes: 120,
        Difficulty: Difficulty.Hard,
        CategoryId: Guid.NewGuid(),
        Ingredients:
        [
            new CreateIngredientDto("Flour", 500, "g"),
            new CreateIngredientDto("Butter", 250, "g")
        ]);

    [Fact]
    public void ValidCommand_ShouldPassValidation()
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand();

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
        CreateRecipeCommand command = ValidCommand() with { Name = name };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Name));
    }

    [Fact]
    public void NameExceeds200Chars_ShouldFailValidation()
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand() with { Name = new string('x', 201) };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ZeroOrNegativePrepTime_ShouldFailValidation(int prepTimeMinutes)
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand() with { PrepTimeMinutes = prepTimeMinutes };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.PrepTimeMinutes));
    }

    [Fact]
    public void EmptyCategoryId_ShouldFailValidation()
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand() with { CategoryId = Guid.Empty };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.CategoryId));
    }

    [Fact]
    public void InvalidDifficulty_ShouldFailValidation()
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand() with { Difficulty = (Difficulty)99 };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Difficulty));
    }

    [Fact]
    public void IngredientWithZeroQuantity_ShouldFailValidation()
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand() with
        {
            Ingredients = [new CreateIngredientDto("Flour", 0, "g")]
        };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Quantity"));
    }

    [Fact]
    public void IngredientWithEmptyName_ShouldFailValidation()
    {
        // Arrange
        CreateRecipeCommand command = ValidCommand() with
        {
            Ingredients = [new CreateIngredientDto("", 100, "g")]
        };

        // Act
        ValidationResult result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Name"));
    }
}
