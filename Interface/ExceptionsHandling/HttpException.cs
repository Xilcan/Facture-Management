using System.Net;

namespace Interface.ExceptionsHandling;

public class HttpException : Exception
{
    public HttpException(string? message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpException(string? message, Exception? innerException, HttpStatusCode statusCode)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; private set; }
}
