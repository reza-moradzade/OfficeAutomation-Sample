using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OfficeAutomationSuite.Client;
using OfficeAutomationSuite.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ---------- Root components ----------
builder.RootComponents.Add<App>("#app");               // Main App component
builder.RootComponents.Add<HeadOutlet>("head::after"); // Head outlet for meta tags, scripts, etc.

// ---------- HttpClient ----------
// Named HttpClient for server API
builder.Services.AddHttpClient("OfficeAutomationSuite.ServerAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Default HttpClient for general use
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// HttpClient factory to supply HttpClient instances with server API base address
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("OfficeAutomationSuite.ServerAPI"));

// ---------- Services ----------
// Register UserService for authentication
builder.Services.AddScoped<UserService>();
// Register CartableService for task management
builder.Services.AddScoped<CartableService>();

// ---------- Run the application ----------
await builder.Build().RunAsync();
