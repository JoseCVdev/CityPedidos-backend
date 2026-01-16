using CityPedidos.Application.Interfaces.Auth;
using CityPedidos.Application.Interfaces.Core;
using CityPedidos.Application.Services.Auth;
using CityPedidos.Application.Services.Core;
using CityPedidos.Domain.Entities.Interfaces;
using CityPedidos.Infrastructure.Auth;
using CityPedidos.Infrastructure.Data;
using CityPedidos.Infrastructure.Repository;
using CityPedidos.Infrastructure.Resilience;
using CityPedidos.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CONFIGURACION LOGGING
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // nivel mínimo global
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning) // solo warnings y errores de EF Core
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// REGISTRO DEL DBCONTEXT
builder.Services.AddDbContext<PedidosDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// CONFIGURACION JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                ok = false,
                status = 401,
                error = "No autorizado. Token inválido o no proporcionado."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        },

        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = new
            {
                ok = false,
                status = 403,
                error = "No tienes permisos para acceder a este recurso."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    };
});

// RATE LIMITER
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Demasiadas solicitudes. Intenta más tarde.", cancellationToken);
    };

    options.AddPolicy("fixed", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5, // Cantidad máxima de requests permitidas por ventana
                Window = TimeSpan.FromSeconds(10), // Duración de la ventana de tiempo para contar los requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Orden de los requests en cola si la cola existe
                QueueLimit = 0 // 0 significa que no hay cola y requests extras se rechazan de inmediato
            }));
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Permite cualquier dominio
              .AllowAnyMethod()   // Permite GET, POST, PUT, DELETE, etc.
              .AllowAnyHeader();  // Permite cualquier cabecera
    });
});

// RESILIENCIA
builder.Services.AddScoped<IResilienceExecutor, ResilienceExecutor>();

// JWT SERVICE
builder.Services.AddScoped<JwtTokenService>();

// REPOSITORIES
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

var app = builder.Build();

// MIDDLEWARE GLOBAL DE EXCEPCIONES
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

// MIGRACION + SEED
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PedidosDbContext>();
    dbContext.Database.Migrate();
    DbSeeder.Seed(dbContext);
}

app.Run();
