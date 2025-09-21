using System.Threading.Tasks;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Interfaces
{
    // Interface for authentication API client
    public interface IAuthClient
    {
        // Authenticate a user with credentials and client/device info
        Task<AuthResultDto> LoginAsync(
            LoginDto loginDto,
            string captchaToken,
            string clientType,
            string ipAddress,
            string deviceInfo
        );

        // Future methods:
        // Task<AuthResultDto> RefreshAsync(string refreshToken);
        // Task LogoutAsync(...);
    }
}
