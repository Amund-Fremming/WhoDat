namespace Backend.Features.Shared.ResultPattern
{
    public static class ResultExtention
    {
        public static ActionResult Resolve<T>(this Result<T> result, Func<Result<T>, OkObjectResult> success, Func<Result<T>, BadRequestObjectResult> failure)
            => result.IsSuccess ? success(result) : failure(result);

        public static void HandleSuccess<T>(this Result<T> result, Action<Result<T>> action)
        {
            if (!result.IsSuccess)
                throw new ResultException("Operation was not successful.");

            action.Invoke(result);
        }

        public static void HandleFailure<T>(this Result<T> result, Action<Result<T>> action)
        {
            if (result.IsSuccess)
                throw new ResultException("Operation was successful, cannot handle failure.");

            action.Invoke(result);
        }

        public static void HandleFailure(this Result result, Action<Result> action)
        {
            if (result.IsSuccess)
                throw new ResultException("Operation was successful, cannot handle failure.");

            action.Invoke(result);
        }

        // new
        public static Result RemoveType<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return Result.Success();

            return Result.Failure(result.Exception!, result.Message);
        }
    }
}