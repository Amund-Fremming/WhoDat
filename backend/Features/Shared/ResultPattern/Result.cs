namespace Backend.Features.Shared.ResultPattern
{
    public record Result<T>(T? Data, string Message = "", Exception? Exception = null) : IResult, IResult<T>
    {
        public bool IsSuccess => Data is not null;

        public static Result<T> Failure(string message, Exception exception) => new(default, message, exception);
        public static Result<T> Success(T data) => new(data);
        public static implicit operator Result<T>(T data) => new(data);
    }

    public record Result(string Message = "", Exception? Exception = null) : IResult
    {
        public bool IsSuccess => Exception == null;

        public static Result Success() => new();
        public static Result Failure(string message, Exception exception) => new(message, exception);
    }
}