using System.Runtime.Serialization;
using DisasterTrackerApp.Utils.Extensions;

namespace DisasterTrackerApp.Models.ApiModels;

public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiError? ResponseException { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse()
        {
            this.IsSuccess = true;
            this.StatusCode = 200;
        }
        public ApiResponse(ApiError apiError, int statusCode = 500)
        {
            this.ResponseException = apiError;
            this.IsSuccess = false;
            this.StatusCode = statusCode;
        }

        public static ApiResponse Success()
            =>
                new ApiResponse()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                };

        public static ApiResponse Failed()
            =>
                new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                };


    }

    public class ApiResponse<T> : ApiResponse

    {
        [DataMember(EmitDefaultValue = false)]
        public T Data { get; set; }

        public ApiResponse() : base() { }
        public ApiResponse(T data) : base()
        {
            this.Data = data;
        }

        public ApiResponse(ApiError error, int statusCode = 500) : base(error, statusCode)
        {
            this.Data = default(T);
        }

        public static ApiResponse<T> InternalError()
            => new ApiResponse<T>()
            {
                StatusCode = 500,
                IsSuccess = false,
                ResponseException = new ApiError(ErrorCode.InternalError, ErrorCode.InternalError.GetDescription()),
            };

        public ApiResponse<TX> ToFailed<TX>()
        where TX : class, new()

            => new ApiResponse<TX>()
            {
                Data = null,
                IsSuccess = false,
                StatusCode = this.StatusCode,
                ResponseException = this.ResponseException,
            };

    }