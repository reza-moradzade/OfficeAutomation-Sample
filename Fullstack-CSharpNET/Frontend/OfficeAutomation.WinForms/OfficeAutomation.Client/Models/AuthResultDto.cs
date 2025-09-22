using System;
using System.Collections.Generic;

namespace OfficeAutomation.Client.Models
{
    public class AuthResultDto
    {
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string Token { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsLocked { get; set; }
        public string? Message { get; set; }
        public int LockRemainingSeconds { get; set; }
        public int FailedLoginCount { get; set; }
    }
}
