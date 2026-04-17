using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApp.API.Middleware;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppBusiness.Implementations;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppRepository.Data;
using QuantityMeasurementAppRepository.Interfaces;
using QuantityMeasurementAppRepository.Repository;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────
//  1. Controllers
// ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ─────────────────────────────────────────────────────────────
//  2. Swagger — with JWT Bearer support
// ─────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantity Measurement API",
        Version = "v1",
        Description = "N-tier REST API secured with JWT Bearer authentication."
    });

    //  UC17: Add Bearer token input to Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token below.\nExample: eyJhbGci..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ─────────────────────────────────────────────────────────────
//  3. Database
// ─────────────────────────────────────────────────────────────
var connectionString = builder.Configuration["DB_CONNECTION_STRING"] 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("CRITICAL: Connection string not found in Environment (DB_CONNECTION_STRING) or appsettings.json.");

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging();   // UC19: log SQL with param values
    options.LogTo(Console.WriteLine,        // UC19: print EF SQL to console
        Microsoft.Extensions.Logging.LogLevel.Information);
});

// ─────────────────────────────────────────────────────────────
//  4. JWT Authentication  UC17
// ─────────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSection["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured in appsettings.json.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero  // No extra grace time on expiry
        };
    });

builder.Services.AddAuthorization();

// ─────────────────────────────────────────────────────────────
//  5. DI — Repositories & Services
// ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementEfRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

//  UC17: Register user repo and auth service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();

// UC19 : 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:4200", 
                "http://localhost:5500",
                "http://localhost:5500",
                "https://127.0.0.1:5500",
                "https://localhost:5500"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


//  6. Build & Middleware Pipeline

var app = builder.Build();

// Auto-migrate DB on startup (creates tables including 'users')
using (var scope = app.Services.CreateScope())
{
    var db     = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    // UC19: Log the exact connection being used so you can verify in SSMS
    var conn = db.Database.GetDbConnection();
    logger.LogWarning("=== DB CONNECTION: Server={Server} | Database={Database} ===",
        conn.DataSource, conn.Database);

    db.Database.Migrate();
    logger.LogWarning("=== MIGRATION COMPLETE. Tables ready. ===");
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();                    // UC19 : CORS must come before HTTPS redirect
app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement v1");
});

// UC17: Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();