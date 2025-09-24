using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using OfficeAutomation.MAUI.Services;

namespace OfficeAutomation.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // ✅ Configure HttpClient with platform-specific BaseAddress
            string apiBaseUrl;

#if ANDROID
            // Android Emulator: use 10.0.2.2 to reach host machine
            apiBaseUrl = "https://10.0.2.2:7169/";
#elif IOS
            // iOS Simulator: use your machine's LAN IP instead of localhost
            // Example: check your IP with `ipconfig` or `ifconfig`
            apiBaseUrl = "https://192.168.1.10:7169/";
#else
            // Windows / Mac (direct execution on host machine)
            apiBaseUrl = "https://localhost:7169/";
#endif

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            });

            // ✅ Register AuthState as Singleton (shared authentication state)
            builder.Services.AddSingleton<AuthState>();

            // ✅ Register application services
            builder.Services.AddScoped<ITokenService, TokenService>();       // Token management
            builder.Services.AddScoped<IAuthService, AuthService>();         // Authentication logic
            builder.Services.AddScoped<ICartableService, CartableService>(); // Cartable operations

            return builder.Build();
        }
    }
}
