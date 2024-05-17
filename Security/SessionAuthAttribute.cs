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
    public SessionAuthAttribute(params Role[] roles) : base(typeof(SessionAuthFilter))
    {
        Arguments = [roles];
    }
}

public class SessionAuthFilter : IAsyncAuthorizationFilter
{
    private readonly SessionRepository _sessionRepository;
    private readonly Role[] _roles;

    public SessionAuthFilter(SessionRepository sessionRepository, Role[] roles)
    {
        _sessionRepository = sessionRepository;
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(Configurations.SessionIdCookieKey, out string? sessionIdStr))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

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

        if (_roles.Length > 0 && !_roles.Any(role => session.Roles.Contains(role)))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items[Configurations.SessionItemsKey] = session;
    }
}
