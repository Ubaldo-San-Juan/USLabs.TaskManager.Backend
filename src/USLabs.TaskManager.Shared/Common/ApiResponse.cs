
namespace USLabs.TaskManager.Shared.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public ApiResponse()
        {
        }

        public ApiResponse(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Errors = errors;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>(message);
        }

        public static ApiResponse<T> ErrorResponse(List<string> errors)
        {
            return new ApiResponse<T>(errors);
        }  
        
              
    }
}