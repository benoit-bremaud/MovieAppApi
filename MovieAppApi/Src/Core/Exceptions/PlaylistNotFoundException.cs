namespace MovieAppApi.Src.Core.Exceptions;

public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException(string message) : base(message) { }
    public PlaylistNotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
}
