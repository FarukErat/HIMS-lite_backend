using Configuration;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Repositories;

namespace Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class SessionAuthAttribute : TypeFilterAttribute
{
    public Role[]? Roles;
    public SessionAuthAttribute() : base(typeof(SessionAuthFilter))
    {
    }
}

public class SessionAuthFilter(
    SessionRepository sessionRepository
) : IAsyncAuthorizationFilter
{
    private readonly SessionRepository _sessionRepository = sessionRepository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string? sessionIdStr = context.HttpContext.Request.Cookies[Configurations.SessionIdCookieKey];
        if (!Guid.TryParse(sessionIdStr, out Guid sessionIdGuid))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        Session? session = await _sessionRepository.GetSessionByIdAsync(sessionIdGuid);
        if (session is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (session.ExpireAt < DateTime.UtcNow)
        {
            await _sessionRepository.DeleteSessionByIdAsync(sessionIdGuid);
            context.Result = new UnauthorizedResult();
            return;
        }

        Role[]? roles = context.Filters.OfType<SessionAuthAttribute>().FirstOrDefault()?.Roles;
        if (roles is not null && roles.Length > 0
            && !roles.Any(session.Roles.Contains))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items[Configurations.SessionItemsKey] = session;
    }
}
