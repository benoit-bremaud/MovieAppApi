namespace MovieAppApi.Src.Core.Exceptions;

public class MovieNotFoundException : Exception
{
    public MovieNotFoundException(string message) : base(message) { }
    public MovieNotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
}
