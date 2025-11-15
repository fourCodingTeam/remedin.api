namespace Remedin.Application.DTOs.Responses
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public BaseResponse() { }

        public BaseResponse(bool success = true, string? message = null, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static BaseResponse<T> Ok(string? message = null, T? data = default)
        {
            return new (true, message, data);
        }

        public static BaseResponse<T> Fail(string? message = null, T? data = default)
        {
            return new(false, message, data);
        }
    }
}
