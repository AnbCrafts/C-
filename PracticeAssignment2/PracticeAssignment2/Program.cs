using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PracticeAssignment2.Data;
using PracticeAssignment2.Middleware;
using PracticeAssignment2.Repositories;
using PracticeAssignment2.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection Factory
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// 2. Repositories (Pure ADO.NET)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICancellationRepository, CancellationRepository>();

// 3. Application Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();

// 4. JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] 
    ?? "ThisIsASecretKeyForHotelBookingCapstoneApplication2026!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "HotelBookingApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "HotelBookingApiUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// 5. Controllers & OpenAPI / Scalar
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 6. Global Exception Middleware (RFC 7807 ProblemDetails)
app.UseMiddleware<GlobalExceptionMiddleware>();

// 7. OpenAPI & Interactive API Documentation
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 8. Authentication & Authorization Pipeline
app.UseAuthentication();
app.UseAuthorization();

// 9. Map API Endpoints
app.MapControllers();

app.Run();
