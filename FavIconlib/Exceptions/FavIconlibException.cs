namespace FavIconlib.Exceptions
{
    internal class FavIconlibException : Exception
    {
        public FavIconlibException() { }

        public FavIconlibException(string message) : base(message) { }

        public FavIconlibException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
