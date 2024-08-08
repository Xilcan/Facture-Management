using System.Net;

namespace Interface.ExceptionsHandling.Exceptions;
public class BadRequestException : HttpException
{
    private static readonly HttpStatusCode _statusCode = HttpStatusCode.BadRequest;

    public BadRequestException(string? message)
        : base(message, _statusCode)
    {
    }

    public BadRequestException(string? message, Exception? innerException)
        : base(message, innerException, _statusCode)
    {
    }
}
