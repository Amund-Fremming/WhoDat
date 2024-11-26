namespace Backend.Features.Shared.ResultPattern
{
    public interface IResult
    {
        Error? Error { get; }
    }

    public interface IResult<out T> : IResult
    {
        T? Data { get; }
    }
}