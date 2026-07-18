// Program.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using SportsFacility.Domain.Infra;
using SportsFacility.Domain.Models;
using SportsFacility.Domain.Services;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/sportsfacility-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Sports Facility Management API",
        Version = "v1",
        Description = "API for managing sports facilities and bookings"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization using Bearer scheme. Enter only your JWT token or 'Bearer {token}' depending on your configuration.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    // Custom JSON serializer settings for Indian context
    // Configure serializer for Swagger UI responses
    // Newtonsoft or System.Text.Json settings for Swagger are handled by AddSwaggerGen/Swashbuckle defaults
});

var jwt = builder.Configuration.GetSection("Jwt");

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

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SportsFacilityDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register services (use concrete interfaces from Domain)
builder.Services.AddScoped<SportsFacility.Domain.Interface.IBookingService, BookingService>();
builder.Services.AddScoped<SportsFacility.Domain.Services.AuthService>();
builder.Services.AddAutoMapper(typeof(Program));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Manager", policy => policy.RequireRole("Manager"));
    options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));
    options.AddPolicy("Member", policy => policy.RequireRole("Member"));
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Add custom middleware
//app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseMiddleware<RequestLoggingMiddleware>();

// Add health check endpoint
app.MapHealthChecks("/health");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SportsFacilityDbContext>();
    context.Database.EnsureCreated();
}

// Configure API behavior
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return;
    }

    await next();
});

app.Run();


