using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using OfficeAutomationSuite.Server.Data;
using OfficeAutomationSuite.Server.Repositories;
using OfficeAutomationSuite.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and Razor pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add database context with SQLite connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories for dependency injection
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CartableRepository>();

// Add services for dependency injection
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CartableService>();

// Configure response compression (optional for Blazor WASM)
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Auto create database and tables if not exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Map Razor pages, controllers and fallback
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Run the application
app.Run();
