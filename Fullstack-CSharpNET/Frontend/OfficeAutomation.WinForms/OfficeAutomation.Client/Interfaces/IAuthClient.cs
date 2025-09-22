using System.Threading.Tasks;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Interfaces
{
    public interface IAuthClient
    {
        Task<AuthResultDto> LoginAsync(LoginDto loginDto,
                                       string captchaToken,
                                       string clientType,
                                       string ipAddress,
                                       string deviceInfo);
        // در آینده: Task<AuthResultDto> RefreshAsync(string refreshToken);
        //            Task LogoutAsync(...);
    }
}
