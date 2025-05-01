using ReactChat.Application.Services.User.Session;
using System.Security.Claims;

public class SessionRevocationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
{
    private readonly RequestDelegate _next = next;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                IServiceProvider serviceProvider = context.RequestServices;
                var scope = _serviceProvider.CreateScope();
                var _sessionService = scope.ServiceProvider.GetRequiredService<SessionService>();
                var userIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var session = await _sessionService.GetCurrentUserSession(userId, userIp);

                if (session?.IsRevoked ?? true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Your session has been revoked.");
                    return;
                }
            }
        }

        await _next(context);
    }
}
