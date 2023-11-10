using System.Net;

namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public static Result<T> Success() => new Result<T> { IsSuccess = true };
        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        public static Result<T> NotFound() => new Result<T> { IsSuccess = true, StatusCode = HttpStatusCode.NotFound };
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error, StatusCode = HttpStatusCode.BadRequest };
    }
}
