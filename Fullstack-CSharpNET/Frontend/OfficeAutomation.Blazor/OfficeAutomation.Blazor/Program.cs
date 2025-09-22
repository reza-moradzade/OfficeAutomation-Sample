using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using OfficeAutomation.Blazor;
using OfficeAutomation.Blazor;
using OfficeAutomation.Blazor.Services;
using OfficeAutomation.Blazor.Services;
using OfficeAutomation.Blazor.Services.Interfaces;
using System.Net.Http;
using IAuthService = OfficeAutomation.Blazor.Services.IAuthService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root Components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient برای API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7169/") // آدرس API
});

// AuthState یکبار Singleton
builder.Services.AddSingleton<AuthState>();

// سرویس‌ها
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartableService, CartableService>();
builder.Services.AddMudServices();
await builder.Build().RunAsync();
