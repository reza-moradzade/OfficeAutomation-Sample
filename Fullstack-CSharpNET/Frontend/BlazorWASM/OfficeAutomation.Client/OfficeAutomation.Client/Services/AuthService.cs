using OfficeAutomation.Client.Services;

namespace OfficeAutomation.Client.Services
{
    public class AuthService : IAuthService
    {
        public Task<bool> LoginAsync(string username, string password)
        {
            // شبیه‌سازی ورود، بعداً با API جایگزین می‌شود
            return Task.FromResult(username == "admin" && password == "123");
        }

        public Task LogoutAsync()
        {
            // شبیه‌سازی خروج
            return Task.CompletedTask;
        }
    }
}
