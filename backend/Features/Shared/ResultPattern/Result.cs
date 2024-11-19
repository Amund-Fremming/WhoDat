namespace Backend.Features.Shared.ResultPattern
{
    // new (rekkefølge på ex og message byttet, er likt logger nå)
    public record Result<T>(T Data, Exception? Exception = null, string Message = "") : IResult, IResult<T>
    {
        public bool IsSuccess => Data is not null;

        public static Result<T> Failure(Exception exception, string message) => new(default!, exception, message);
        public static Result<T> Failure(string message) => new(default!, null, message);
        public static Result<T> Success(T data) => new(data);
        public static implicit operator Result<T>(T data) => new(data);
    }

    public record Result(Exception? Exception = null, string Message = "") : IResult
    {
        public bool IsSuccess => Exception == null;

        public static Result Success() => new();
        public static Result Failure(Exception exception, string message) => new(exception, message);
        // new
        public static Result Failure(string message) => new(null, message);
    }
}