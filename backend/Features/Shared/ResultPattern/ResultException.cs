namespace Backend.Features.Shared.ResultPattern
{
    internal class ResultException(string message) : Exception(message)
    {
    }
}