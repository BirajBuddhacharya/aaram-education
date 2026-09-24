using System.Security.Claims;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Web.Models.ViewModels.Gamification;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

public class BadgesController(IBadgeRepository badges, IUserRepository users) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var all = await badges.GetAllAsync();
        var studentId = GetStudentId();

        IEnumerable<AaramEducation.Core.Entities.UserBadge> earned = [];
        if (studentId.HasValue)
            earned = await badges.GetUserBadgesAsync(studentId.Value);

        var earnedIds = earned.ToDictionary(ub => ub.BadgeId, ub => ub);

        var items = all.Select(b => new BadgeDisplayItem
        {
            Badge = b,
            Earned = earnedIds.ContainsKey(b.BadgeId),
            EarnedAt = earnedIds.TryGetValue(b.BadgeId, out var ub) ? ub.EarnedAt : null,
        }).ToList();

        return View(new BadgesViewModel
        {
            Badges = items,
            EarnedCount = items.Count(i => i.Earned),
            TotalCount = items.Count,
        });
    }

    [HttpGet]
    public async Task<IActionResult> Leaderboard()
    {
        var all = await users.GetAllAsync();
        var studentId = GetStudentId();

        var ranked = all
            .Where(u => u.Role == UserRole.Student)
            .OrderByDescending(u => u.TotalXpPoints)
            .Select((u, i) => new LeaderboardEntry { Rank = i + 1, User = u, XpPoints = u.TotalXpPoints })
            .ToList();

        int? myRank = studentId.HasValue
            ? ranked.FirstOrDefault(e => e.User.UserId == studentId.Value)?.Rank
            : null;

        return View(new LeaderboardViewModel
        {
            Entries = ranked.Take(50),
            CurrentUserRank = myRank,
        });
    }

    private int? GetStudentId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
