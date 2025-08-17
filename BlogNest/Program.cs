using System.Text;
using BlogNest.Data;
using BlogNest.Middlewares;
using BlogNest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(); // Use Serilog for logging
// Add services to the container
builder.Services.AddControllers(); // ✅ Essential for API Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Instead of builder.Services.AddOpenApi()

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:5173") // Adjust to your frontend URL
              .AllowCredentials();
    });
});
// ✅ Register custom services BEFORE Build()
builder.Services.AddScoped<IAuthService, AuthService>(); // ✔ Make sure this matches actual class name
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true); // Optional: Use original property names
// JWT Authentication setup
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ⚠️ Must be before UseAuthorization
app.UseAuthorization();
app.UseStaticFiles();
app.UseCors("AllowFrontEnd"); // ✅ Apply CORS policy
app.UseMiddleware<ErrorHandlingMiddleware>(); // ✅ Custom error handling middleware
app.MapControllers(); // ✅ Route to controllers like AuthController

app.Run();
