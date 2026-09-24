using System.Security.Claims;
using AaramEducation.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Components.NavbarNotifications;

public class NavbarNotificationsViewComponent(INotificationRepository notifications) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (UserClaimsPrincipal.Identity?.IsAuthenticated != true)
            return Content(string.Empty);

        var raw = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(raw, out var userId))
            return Content(string.Empty);

        var recent = await notifications.GetRecentAsync(userId, 5);
        var unread = await notifications.GetUnreadCountAsync(userId);

        ViewBag.UnreadCount = unread;
        return View(recent);
    }
}
