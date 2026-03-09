using System.Net;
using System.Net.Http.Json;
using Application.Features.Categories.Queries;
using FluentAssertions;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Controllers;

/// <summary>Integration tests for <c>CategoriesController</c>.</summary>
public sealed class CategoriesControllerTests(AppFactory factory)
    : IClassFixture<AppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_ReturnsEmptyList()
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        List<CategoryDto>? categories =
            await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        categories.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WithValidName_ReturnsCreated()
    {
        // Arrange
        var payload = new { name = "Tarts" };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var payload = new { name = "" };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Update_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        Guid unknownId = Guid.NewGuid();
        var payload = new { Id = unknownId, name = "Renamed" };

        // Act
        HttpResponseMessage response =
            await _client.PutAsJsonAsync($"/api/categories/{unknownId}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        Guid unknownId = Guid.NewGuid();

        // Act
        HttpResponseMessage response =
            await _client.DeleteAsync($"/api/categories/{unknownId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
