using Hangfire.Dashboard;
using KST.Business.Interfaces;
using KST.DataAccess.Enums;

namespace KST.WEB.Infrastructure;

public sealed class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        var currentUserService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
        var userService = httpContext.RequestServices.GetRequiredService<IUserService>();
        var httpUserName = currentUserService.GetCurrentUserAsync(CancellationToken.None).GetAwaiter().GetResult();
        if (httpUserName?.Id == null)
        {
            return false;
        }

        var user = userService.GetByIdAsync(httpUserName.Id.Value, CancellationToken.None).GetAwaiter().GetResult();
        return user.Role == UserRoles.Admin;
    }
}