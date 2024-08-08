using System.Net;
using System.Text;
using System.Text.Json;
using Interface.ExceptionsHandling;
using Interface.Logger;

namespace Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IFileLoggerService _loggerService;

    public ExceptionMiddleware(RequestDelegate next, IFileLoggerService loggerService)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(loggerService);

        _next = next;
        _loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpException ex)
        {
            var log = await HandleHttpExceptionAsync(context, ex);
            await _loggerService.LogExceptionAsync(log);
        }
        catch (Exception ex)
        {
            var log = await HandleExceptionAsync(context, ex);
            await _loggerService.LogExceptionAsync(log);
        }
    }

    private static async Task<string> HandleHttpExceptionAsync(HttpContext context, HttpException exception)
    {
        await HandleAllExceptionsAsync(context, exception.Message, exception.StatusCode);
        return LogException(exception);
    }

    private static async Task<string> HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var message = exception.Message ?? "Unexpected error occured";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        if (exception is UnauthorizedAccessException)
        {
            message = "Unauthorized access.";
            statusCode = HttpStatusCode.Unauthorized;
        }

        await HandleAllExceptionsAsync(context, message, statusCode);
        return LogException(exception);
    }

    private static Task HandleAllExceptionsAsync(HttpContext context, string message, HttpStatusCode statusCode)
    {
        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }

    private static string LogException(Exception exception)
    {
        var logDetails = new StringBuilder()
            .AppendLine($"Exception Type: {exception.GetType()}")
            .AppendLine($"Exception Message: {exception.Message}")
            .AppendLine($"Stack Trace: {exception.StackTrace}");

        var innerException = exception.InnerException;
        while (innerException != null)
        {
            logDetails.AppendLine($"Inner Exception Type: {innerException.GetType()}")
                      .AppendLine($"Inner Exception Message: {innerException.Message}")
                      .AppendLine($"Inner Exception Stack Trace: {innerException.StackTrace}");
            innerException = innerException.InnerException;
        }

        return logDetails.ToString();
    }
}
