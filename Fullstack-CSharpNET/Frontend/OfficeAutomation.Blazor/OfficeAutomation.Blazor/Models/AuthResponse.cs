namespace OfficeAutomation.Blazor.Models
{
    public class AuthResponse
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
        public List<string> Roles { get; set; }
        public string RefreshToken { get; set; }
    }
}
