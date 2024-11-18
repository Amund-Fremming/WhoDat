namespace Backend.Features.Game;

public class GameFullException : Exception
{
    public GameFullException()
    { }

    public GameFullException(string message) : base(message)
    {
    }
}