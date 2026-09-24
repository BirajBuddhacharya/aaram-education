using System.Security.Claims;
using AaramEducation.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

[Authorize]
public class NotificationsController(INotificationRepository notifications) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId()!.Value;
        var all = await notifications.GetRecentAsync(userId, 50);
        return View(all);
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkRead(int id)
    {
        var userId = GetUserId()!.Value;
        await notifications.MarkReadAsync(id, userId);
        return Ok();
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllRead()
    {
        var userId = GetUserId()!.Value;
        await notifications.MarkAllReadAsync(userId);
        return RedirectToAction("Index");
    }

    private int? GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
