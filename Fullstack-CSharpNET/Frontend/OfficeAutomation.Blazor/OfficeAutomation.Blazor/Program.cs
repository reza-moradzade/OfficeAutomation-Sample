using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using OfficeAutomation.Blazor;
using OfficeAutomation.Blazor.Services;
using IAuthService = OfficeAutomation.Blazor.Services.IAuthService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root Components
// The main application component and HeadOutlet for managing the document head
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7169/") // Base URL for the backend API
});

// Register AuthState as a Singleton to maintain authentication state across the app
builder.Services.AddSingleton<AuthState>();

// Register application services with scoped lifetime
builder.Services.AddScoped<ITokenService, TokenService>();       // Handles token management
builder.Services.AddScoped<IAuthService, AuthService>();         // Handles authentication logic
builder.Services.AddScoped<ICartableService, CartableService>(); // Handles cartable-related operations

// Register MudBlazor services for UI components
builder.Services.AddMudServices();

// Build and run the application
await builder.Build().RunAsync();
