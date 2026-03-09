using FluentAssertions;
using IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Endpoints;

/// <summary>
/// Integration tests for the Samples endpoints.
/// These tests run against a real SQL Server container (via Testcontainers).
/// Docker must be running on the host machine for these tests to work.
/// </summary>
public sealed class SamplesEndpointsTests(AppFactory factory)
    : IClassFixture<AppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetSamples_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/samples");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateSample_ValidRequest_ReturnsCreated()
    {
        var payload = new { name = "Test Sample" };
        var response = await _client.PostAsJsonAsync("/api/samples", payload);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateSample_EmptyName_ReturnsBadRequest()
    {
        var payload = new { name = "" };
        var response = await _client.PostAsJsonAsync("/api/samples", payload);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
