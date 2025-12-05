using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de Prometheus para m�tricas HTTP (latencia, status codes, etc.)
app.UseHttpMetrics();

app.UseAuthorization();

// ---- ENDPOINTS EXTRA ----

// 1) /env -> ver entorno
app.MapGet("/env", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        environment = env.EnvironmentName,  // Development, Staging, Production, etc.
        application = env.ApplicationName
    });
});

// 2) /version -> lee el archivo VERSION del output
app.MapGet("/version", () =>
{
    var versionFilePath = Path.Combine(AppContext.BaseDirectory, "VERSION");

    if (!File.Exists(versionFilePath))
    {
        return Results.NotFound(new
        {
            error = "VERSION file not found",
            path = versionFilePath
        });
    }

    var version = File.ReadAllText(versionFilePath).Trim();
    return Results.Ok(new { version });
});

// 3) /health -> healthcheck b�sico
app.MapHealthChecks("/health");

// 4) /metrics -> endpoint de Prometheus
app.MapMetrics("/metrics");

// Controllers existentes
app.MapControllers();

app.Run();
