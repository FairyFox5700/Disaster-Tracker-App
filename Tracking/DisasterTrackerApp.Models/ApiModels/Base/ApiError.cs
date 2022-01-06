using System.ComponentModel;

namespace DisasterTrackerApp.Models.ApiModels.Base;

public class ApiError
{
    public ErrorCode ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    
    public ApiError(ErrorCode errorCode, string errorMessage)
    {
        this.ErrorCode = errorCode;
        this.ErrorMessage = errorMessage??"";
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
