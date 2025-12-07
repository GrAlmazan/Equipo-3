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
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Project.Infrastructure.DbFactory;
using Project.Infrastructure.Generic;
// 👇 NUEVOS USINGS PARA VALIDACIÓN
using FluentValidation;
using Project.Application.Features.Products.Create; 

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Controllers y Explorador de API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventario - Equipo 3", Version = "v1" });

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
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// 3. SEGURIDAD JWT
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

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

// 4. CONFIGURACIÓN MEDIATR Y PIPELINES (¡Aquí está el cambio!)
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly);
    
    // 👇 Registrar Pipelines en ORDEN:
    // 1. Validación (Revisa reglas antes de procesar)
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
    // 2. Interactor (Logs y manejo de errores)
    cfg.AddOpenBehavior(typeof(InteractorPipeline<,>));
});

// 👇 Registrar AUTOMÁTICAMENTE todos los validadores de la capa Application
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);


// 5. INYECCIÓN DE DEPENDENCIAS
builder.Services.AddScoped<ISqlDbConnectionFactory, SqlDbConnectionFactory>();
builder.Services.AddScoped<IGenericDB, GenericDB>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Health Checks
builder.Services.AddHealthChecks();

// 6. RATE LIMITING
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

var app = builder.Build();

// Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpMetrics(); // Prometheus
app.UseRateLimiter(); 

app.UseAuthentication(); 
app.UseAuthorization();  

// Endpoints Auxiliares
app.MapGet("/env", (IHostEnvironment env) => Results.Ok(new { environment = env.EnvironmentName }));
app.MapGet("/version", () => Results.Ok(new { version = "1.0.0-final" }));

app.MapHealthChecks("/health");
app.MapMetrics("/metrics");

app.MapControllers();

app.Run();