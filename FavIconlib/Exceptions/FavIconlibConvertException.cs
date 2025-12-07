namespace FavIconlib.Exceptions
{
    internal class FavIconlibConvertException : Exception
    {
       public FavIconlibConvertException() { }

       public FavIconlibConvertException(string message) : base(message) { }

       public FavIconlibConvertException(string message, Exception innerException) : base(message, innerException) { }
    }
}
