using System.Net;

namespace Interface.ExceptionsHandling.Exceptions;

public class DatabaseException : HttpException
{
    private static readonly HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;

    public DatabaseException(string? message)
        : base(message, _statusCode)
    {
    }

    public DatabaseException(string? message, Exception? innerException)
        : base(message, innerException, _statusCode)
    {
    }
}
