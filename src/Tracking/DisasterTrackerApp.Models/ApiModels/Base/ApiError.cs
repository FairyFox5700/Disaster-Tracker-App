using System.ComponentModel;

namespace DisasterTrackerApp.Models.ApiModels.Base;

public class ApiError
{
    public ErrorCode ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    
    public ApiError(ErrorCode errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage ?? string.Empty;
    }
}

public enum ErrorCode
{
    [Description("Internal error")]
    InternalError,
        
    [Description("Invalid request format")]
    InvalidRequestFormat,
    
    [Description("Login failed")]
    LoginError
}