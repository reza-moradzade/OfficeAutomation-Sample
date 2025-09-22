namespace OfficeAutomation.Blazor.Services
{
    public class AuthState
    {
        public bool IsAuthenticated { get; set; } = false;
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
    }
}
