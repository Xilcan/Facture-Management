using System.Net;

namespace Interface.ExceptionsHandling.Exceptions;

public class ServiceException : HttpException
{
    private static readonly HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;

    public ServiceException(string? message)
        : base(message, _statusCode)
    {
    }

    public ServiceException(string? message, Exception? innerException)
        : base(message, innerException, _statusCode)
    {
    }
}
