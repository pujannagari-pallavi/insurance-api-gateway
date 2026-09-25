var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()?
    .Where(origin => Uri.TryCreate(origin, UriKind.Absolute, out _))
    .ToArray() ?? [];

if (builder.Environment.IsProduction() && !allowedOrigins.Any())
{
    throw new InvalidOperationException(
        "Configure Cors__AllowedOrigins__0 with the public frontend origin before starting the gateway.");
}

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");
app.MapReverseProxy();
app.MapGet("/health", () => Results.Ok(new
{
    service = "api-gateway",
    status = "healthy",
    timestampUtc = DateTime.UtcNow
}));

app.Run();