namespace MovieAppApi.Src.Core.Exceptions;

public class TmdbApiException : Exception
{
    public int? StatusCode { get; set; }

    public TmdbApiException(string message, int? statusCode = null) : base(message) 
    {
        StatusCode = statusCode;
    }
}
