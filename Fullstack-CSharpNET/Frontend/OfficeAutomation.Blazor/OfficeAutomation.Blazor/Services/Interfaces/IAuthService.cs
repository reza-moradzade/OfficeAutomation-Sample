using System.Threading.Tasks;
using OfficeAutomation.Blazor.Models;

namespace OfficeAutomation.Blazor.Services.Interfaces
{
    public interface IAuthService
    {
     Task<AuthResponse?> LoginAsync(string username, string password);
        Task LogoutAsync();

    }
}
