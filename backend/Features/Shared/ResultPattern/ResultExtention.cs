namespace Backend.Features.Shared.ResultPattern
{
    public static class ResultExtention
    {
        public static ActionResult Resolve<T>(this Result<T> result, Func<Result<T>, ActionResult> success, Func<Result<T>, ActionResult> error)
            => result.IsError ? error(result) : success(result);

        public static ActionResult Resolve(this Result result, Func<Result, ActionResult> success, Func<Result, ActionResult> error)
            => result.IsError ? error(result) : success(result);

        /// <summary>
        /// Changes type
        /// </summary>
        // public static Result<U> ToResult<T, U>(this Result<T> result) => new(default!, result.Error!);

        /// <summary>
        /// Adds type
        /// </summary>
        public static Result<T> ToResult<T>(this Result result) => new(default!, result.Error!);

        /// <summary>
        /// Removed type
        /// </summary>
        public static Result ToResult<T>(this Result<T> result) => new(result.Error);
    }
}