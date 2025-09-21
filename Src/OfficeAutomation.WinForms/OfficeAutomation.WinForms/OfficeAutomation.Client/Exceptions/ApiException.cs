using System;

namespace OfficeAutomation.Client.Exceptions
{
    // Custom exception for API errors
    public class ApiException : Exception
    {
        public int StatusCode { get; }  // HTTP status code from API response

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
