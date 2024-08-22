using Microsoft.AspNetCore.Authorization;

namespace Api.Middleware;

public class CompanyAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public CompanyAuthorizationMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var authorizeMetadata = endpoint?.Metadata.GetMetadata<IAuthorizeData>();

        if (authorizeMetadata != null)
        {
            var user = context.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            var companyId = user.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;

            if (string.IsNullOrEmpty(companyId))
            {
                context.Response.StatusCode = 403; // Forbidden
                return;
            }

            context.Items["CompanyId"] = companyId;
        }

        await _next(context);
    }
}