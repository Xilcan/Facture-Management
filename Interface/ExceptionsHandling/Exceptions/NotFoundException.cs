using System.Net;

namespace Interface.ExceptionsHandling.Exceptions;

public class NotFoundException : HttpException
{
    private static readonly HttpStatusCode _statusCode = HttpStatusCode.NotFound;

    public NotFoundException(string? message)
        : base(message, _statusCode)
    {
    }

    public NotFoundException(string? message, Exception? innerException)
        : base(message, innerException, _statusCode)
    {
    }
}
