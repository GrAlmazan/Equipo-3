using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Prometheus;
using Common.CleanArch;
using Project.Domain.Repositories;
using Project.Infrastructure.Repositories;
using Project.Infrastructure.Authentication;
using Project.Application.Interfaces;
using Project.Application.Features.Login;
using System.Threading.RateLimiting; // <<--- ¡NUEVA IMPORTACIÓN REQUERIDA!\
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Controllers y Explorador de API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Configurar Swagger para que acepte el botón "Authorize" (Candado)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventario - Equipo 3", Version = "v1" });

    // Definir seguridad JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// 👇👇👇 3. CONFIGURACIÓN DE SEGURIDAD JWT (NUEVO) 👇👇👇

// A) Registrar el generador de tokens
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

// B) Configurar la validación del token (Authentication)
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

// 👆👆👆 FIN CONFIGURACIÓN JWT 👆👆👆


// 4. Configurar MediatR (Busca los comandos en la capa Application)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly));

// 5. Inyección de Dependencias (Repositorios)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Health Checks
builder.Services.AddHealthChecks();

// 👇👇👇 6. CONFIGURACIÓN DE RATE LIMITING (NUEVO) 👇👇👇
// 6. CONFIGURACIÓN DE RATE LIMITING
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed-limit", fixedOptions =>
    {
        fixedOptions.PermitLimit = 10;
        fixedOptions.Window = TimeSpan.FromSeconds(10);
        fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        fixedOptions.QueueLimit = 5;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// 👆👆👆 FIN CONFIGURACIÓN RATE LIMITING 👆👆👆

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de Prometheus
app.UseHttpMetrics();

// 👇👇👇 ACTIVAR RATE LIMITING (ANTES DE AUTH/AUTHZ) 👇👇👇
app.UseRateLimiter(); 

// 7. ACTIVAR SEGURIDAD (ORDEN IMPORTANTE)
app.UseAuthentication(); // <--- ¡Primero identificas quién es!
app.UseAuthorization();  // <--- ¡Luego verificas si tiene permiso!


// ---- ENDPOINTS EXTRA ----

app.MapGet("/env", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        environment = env.EnvironmentName,
        application = env.ApplicationName
    });
});

app.MapGet("/version", () =>
{
    var versionFilePath = Path.Combine(AppContext.BaseDirectory, "VERSION");

    if (!File.Exists(versionFilePath))
    {
        return Results.NotFound(new { error = "VERSION file not found", path = versionFilePath });
    }

    var version = File.ReadAllText(versionFilePath).Trim();
    return Results.Ok(new { version });
});

app.MapHealthChecks("/health");
app.MapMetrics("/metrics");

app.MapControllers();

app.Run();