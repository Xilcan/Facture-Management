using System.Diagnostics;
using System.Text;
using Interface.Logger;

namespace Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IFileLoggerService _loggerService;

    public RequestLoggingMiddleware(RequestDelegate next, IFileLoggerService loggerService)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(loggerService);
        _next = next;
        _loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;
        var requestBodyContent = await ReadRequestBody(request);

        var originalResponseBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        stopwatch.Stop();

        var responseBodyContent = await ReadResponseBody(context.Response);
        var logDetails = new StringBuilder()
            .AppendLine($"IP Address: {context.Connection.RemoteIpAddress}")
            .AppendLine($"Target URL: {request.Path}")
            .AppendLine($"Request Content: {requestBodyContent}")
            .AppendLine($"Response Status Code: {context.Response.StatusCode}")
            .AppendLine($"Response Content: {responseBodyContent}")
            .AppendLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

        await _loggerService.LogRequestAsync(logDetails.ToString());

        await responseBody.CopyToAsync(originalResponseBodyStream);
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();

        var buffer = new byte[1024];
        var bytesRead = await request.Body.ReadAsync(buffer);
        var requestBody = new StringBuilder();

        while (bytesRead > 0)
        {
            requestBody.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            bytesRead = await request.Body.ReadAsync(buffer);
        }

        request.Body.Position = 0;

        return requestBody.ToString();
    }

    private static async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }
}
