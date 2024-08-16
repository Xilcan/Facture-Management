using System.Net;

namespace Interface.ExceptionsHandling.Exceptions;

public class ValidationException : HttpException
{
    private static readonly HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;

    public ValidationException(string? message)
        : base(message, _statusCode)
    {
    }

    public ValidationException(string? message, Exception? innerException)
        : base(message, innerException, _statusCode)
    {
    }
}
