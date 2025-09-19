using System;

namespace OfficeAutomation.Core.DTOs
{
    // DTO used to request user logout
    // Contains the session identifier to be terminated
    public class LogoutDto
    {
        // Unique identifier of the active user session to be revoked
        public Guid SessionId { get; set; }
    }
}
