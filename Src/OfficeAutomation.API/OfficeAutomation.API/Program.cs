using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Implementations;
using OfficeAutomation.Repositories.Interfaces;
using OfficeAutomation.Services.Implementations;
using OfficeAutomation.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Configure DbContext
// ==========================
builder.Services.AddDbContext<OfficeAutomationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OfficeAutomationDB")));

// ==========================
// JWT Authentication setup
// ==========================
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ==========================
// Add controllers and Swagger
// ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================
// Register Repositories
// ==========================
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IFailedLoginAttemptRepository, FailedLoginAttemptRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ICartableRepository, CartableRepository>();

// ==========================
// Register Services
// ==========================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartableService, CartableService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

// ==========================
// Configure CORS policy
// ==========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ==========================
// Middleware pipeline
// ==========================
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OfficeAutomation.API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ==========================
// Map controllers
// ==========================
app.MapControllers();

// ==========================
// Run the application
// ==========================
app.Run();
