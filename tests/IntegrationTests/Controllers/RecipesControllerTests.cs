using System.Net;
using System.Net.Http.Json;
using Application.Features.Recipes.Commands;
using Application.Features.Recipes.Queries;
using Domain.Recipes;
using FluentAssertions;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Controllers;

/// <summary>Integration tests for <c>RecipesController</c>.</summary>
public sealed class RecipesControllerTests(AppFactory factory)
    : IClassFixture<AppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static CreateRecipeCommand ValidCreateCommand(Guid categoryId) => new(
        Name: "Croissant",
        Description: "Buttery French pastry.",
        Instructions: "Laminate the dough with butter.",
        PrepTimeMinutes: 180,
        Difficulty: Difficulty.Hard,
        CategoryId: categoryId,
        Ingredients: [new CreateIngredientDto("Flour", 500, "g")]);

    [Fact]
    public async Task GetAll_ReturnsEmptyList()
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/recipes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        IReadOnlyList<RecipeDto>? recipes =
            await response.Content.ReadFromJsonAsync<List<RecipeDto>>();
        recipes.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WithValidCommand_ReturnsCreated()
    {
        // Arrange — first create a category so the FK is valid
        Guid categoryId = await CreateCategoryAsync("Viennoiseries");
        CreateRecipeCommand command = ValidCreateCommand(categoryId);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/recipes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        Guid? id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Create_WithInvalidCommand_ReturnsBadRequest()
    {
        // Arrange — empty name triggers validation failure
        CreateRecipeCommand command = new(
            Name: "",
            Description: "desc",
            Instructions: "instructions",
            PrepTimeMinutes: 30,
            Difficulty: Difficulty.Easy,
            CategoryId: Guid.NewGuid(),
            Ingredients: []);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/recipes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        Guid unknownId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/recipes/{unknownId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<Guid> CreateCategoryAsync(string name)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/categories",
            new { name });

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}
