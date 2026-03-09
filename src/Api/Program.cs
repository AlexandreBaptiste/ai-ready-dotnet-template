using Api.Middleware;
using Application;
using Infrastructure;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// -- Application & Infrastructure -----------------------------------------------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// -- OpenAPI (Scalar / Swagger) --------------------------------------------------
builder.Services.AddOpenApi();

// -- OpenTelemetry ---------------------------------------------------------------
var serviceName = builder.Configuration["OpenTelemetry:ServiceName"] ?? "Api";

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(serviceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

        if (builder.Environment.IsDevelopment())
            tracing.AddConsoleExporter();

        var otlpEndpoint = builder.Configuration["OpenTelemetry:OtlpEndpoint"];
        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
            tracing.AddOtlpExporter(opts => opts.Endpoint = new Uri(otlpEndpoint));
    });

// -- Controllers -----------------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

// -- Problem Details (RFC 9457) --------------------------------------------------
builder.Services.AddProblemDetails();

// -- CORS (configure as needed) --------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                    ?? ["http://localhost:3000"])
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// -------------------------------------------------------------------------------
var app = builder.Build();
// -------------------------------------------------------------------------------

// -- Middleware pipeline ---------------------------------------------------------
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();

// -- OpenAPI UI (Scalar): only in non-production by default ---------------------
if (!app.Environment.IsProduction())
{
    app.MapOpenApi();                        // exposes /openapi/v1.json
    app.MapScalarApiReference(opts =>        // exposes /scalar/v1
    {
        opts.Title = "API Reference";
        opts.Theme = ScalarTheme.Purple;
    });
}

// -- Controllers ----------------------------------------------------------------
app.MapControllers();

// -- Health check ---------------------------------------------------------------
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTimeOffset.UtcNow }))
   .ExcludeFromDescription();

app.Run();

// Make Program accessible to integration test WebApplicationFactory
public partial class Program { }