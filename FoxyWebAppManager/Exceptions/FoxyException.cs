namespace FoxyWebAppManager.Exceptions;

public class FoxyException : Exception
{
    public FoxyException() { }

    public FoxyException(string message) : base(message) { }

    public FoxyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
