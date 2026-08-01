// Program.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
using Serilog;
using SportsFacility.Domain.Infra;
using SportsFacility.Infrastructure.Data;
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
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("RenderCorsPolicy", policy =>
    {
        policy.WithOrigins("https://sportsfacility.onrender.com", "https://localhost:7265") // No trailing slash!
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Required if you pass cookies, tokens, or sessions
    });
});


// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Distributed Caching configuration (Redis or fallback Memory Cache)
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "SportsFacility_";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

// Register services (use concrete interfaces from Domain)
builder.Services.AddScoped<SportsFacility.Domain.Interface.IAuthService, SportsFacility.Domain.Services.AuthService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IMembershipService, SportsFacility.Domain.Services.MembershipService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IBookingService, SportsFacility.Domain.Services.BookingService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IPaymentService, SportsFacility.Domain.Services.PaymentService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IClassService, SportsFacility.Domain.Services.ClassService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IFacilityService, SportsFacility.Domain.Services.FacilityService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IPlanService, SportsFacility.Domain.Services.PlanService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IStaffService, SportsFacility.Domain.Services.StaffService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IUserService, SportsFacility.Domain.Services.UserService>();
builder.Services.AddScoped<SportsFacility.Domain.Interface.IDashboardService, SportsFacility.Domain.Services.DashboardService>();
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKey12345678901234567890123456789012"))
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
app.UseCors("RenderCorsPolicy");
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
    var context = services.GetRequiredService<ApplicationDbContext>();
    // context.Database.EnsureCreated(); // Will use EF Migrations instead
}

// Configure API behavior
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");

    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return;
    }

    await next();
});

app.MapControllers();
app.Run();


