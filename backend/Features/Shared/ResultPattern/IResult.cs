namespace Backend.Features.Shared.ResultPattern
{
    public interface IResult
    {
        string? Message { get; }
        Exception? Exception { get; }
    }

    public interface IResult<out T> : IResult
    {
        T? Data { get; }
    }
}