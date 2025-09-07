/// <summary>
/// Main entry point and configuration file for the BlogNest application.
/// This file sets up the web application, configures services, and defines the middleware pipeline.
/// </summary>
using System.Text;
using BlogNest.Data;
using BlogNest.Middlewares;
using BlogNest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Configure Serilog for application-wide logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Initialize the web application builder
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog as the logging provider
builder.Host.UseSerilog();

// Configure essential services
builder.Services.AddControllers(); // Required for API Controllers
builder.Services.AddEndpointsApiExplorer(); // Enable API explorer for Swagger
builder.Services.AddSwaggerGen(); // Add Swagger documentation generation

// Configure PostgreSQL database context
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Configure CORS policy to allow frontend application access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:5173") // Frontend application URL
              .AllowCredentials();
    });
});

// Register application services
builder.Services.AddScoped<IAuthService, AuthService>(); // Authentication service registration
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true); // Optional: Use original property names
// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? 
    throw new InvalidOperationException("JWT SecretKey is not configured");
var issuer = jwtSettings["Issuer"] ?? 
    throw new InvalidOperationException("JWT Issuer is not configured");
var audience = jwtSettings["Audience"] ?? 
    throw new InvalidOperationException("JWT Audience is not configured");

// Set up JWT Bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validate the JWT Token's issuer, audience, lifetime, and signing key
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI for API documentation in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable CORS with the configured policy
// This must come before authentication/authorization
app.UseCors("AllowFrontEnd"); 

// Enable authentication and authorization
app.UseAuthentication(); 
app.UseAuthorization(); 

// Serve static files (e.g., images, JavaScript, CSS)
app.UseStaticFiles();

// Add global error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Map controller endpoints
app.MapControllers();

// Start the application
app.Run();
