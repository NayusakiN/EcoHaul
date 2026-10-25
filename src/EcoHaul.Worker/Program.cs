using EcoHaul.Application;
using EcoHaul.Infrastructure;
using EcoHaul.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("EcoHaul")
    ?? throw new InvalidOperationException("Connection string 'EcoHaul' is missing."));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
