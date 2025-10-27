using BookStore.API.Configurations;
using BookStore.API.GraphQL;
using BookStore.Core.Repositories;
using BookStore.Infrastructure.Configurations;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Repositories;
using MongoDB.Driver;
using Serilog;

// Configure Serilog for structured logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/bookstore-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting BookStore API");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog for logging
    builder.Host.UseSerilog();

    // Configuration validation
    var mongoConfig = builder.Configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();
    if (mongoConfig == null || string.IsNullOrEmpty(mongoConfig.ConnectionString))
    {
        throw new InvalidOperationException("MongoDbConfiguration is required in appsettings.json");
    }
    builder.Services.AddSingleton(mongoConfig);

    // MongoDB singleton pattern
    builder.Services.AddSingleton<IMongoClient>(sp =>
    {
        var config = sp.GetRequiredService<MongoDbConfiguration>();
        return new MongoClient(config.ConnectionString);
    });

    // Database context
    builder.Services.AddSingleton<IBookContext, BookContext>();

    // Repositories
    builder.Services.AddScoped<IBookRepository, BookRepository>();

    // CORS configuration
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularApp", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });

    // GraphQL
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>();

    // Health checks
    builder.Services.AddHealthChecks()
        .AddMongoDb(
            sp => sp.GetRequiredService<IMongoClient>(),
            name: "mongodb",
            timeout: TimeSpan.FromSeconds(3),
            tags: new[] { "ready", "db" });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Global exception handling for production
        app.UseExceptionHandler("/error");
    }

    // Enable CORS before routing
    app.UseCors("AllowAngularApp");

    app.UseRouting();

    // Health check endpoints
    app.MapHealthChecks("/health");
    app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready")
    });

    app.MapGraphQL();

    // Error handling endpoint
    app.MapGet("/error", () => Results.Problem("An error occurred processing your request"));

    Log.Information("BookStore API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
