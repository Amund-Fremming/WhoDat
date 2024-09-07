namespace ExceptionNamespace;

public class GameFullException : Exception
{
    public GameFullException() { }

    public GameFullException(string message) : base(message) { }
}
