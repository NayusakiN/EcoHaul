using System.Text.Json.Serialization;
using EcoHaul.Api.Endpoints;
using EcoHaul.Api.Infrastructure;
using EcoHaul.Application;
using EcoHaul.Infrastructure;
using EcoHaul.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<RequestBindingExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.Converters.Add(new OptionalJsonConverterFactory());
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(RequireConnectionString(builder.Configuration));
builder.Services.AddHealthChecks()
    .AddDbContextCheck<EcoHaulDbContext>();
var app = builder.Build();

await EcoHaulDbInitializer.ApplyAsync(app.Services);

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapGet("/health", async (EcoHaulDbContext db, CancellationToken ct) =>
    await db.Database.CanConnectAsync(ct)
        ? Results.Ok()
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable));
app.MapPickupEndpoints();

app.Run();

static string RequireConnectionString(IConfiguration configuration) =>
    configuration.GetConnectionString("EcoHaul")
    ?? throw new InvalidOperationException("Connection string 'EcoHaul' is missing.");

public partial class Program;
