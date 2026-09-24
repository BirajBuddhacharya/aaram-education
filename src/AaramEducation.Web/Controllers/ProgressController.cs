using System.Security.Claims;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.Models.ViewModels.Progress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Web.Controllers;

[Authorize]
public class ProgressController(
    IUserRepository users,
    IEnrollmentRepository enrollments,
    IBadgeRepository badges,
    ApplicationDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var studentId = GetStudentId();
        if (studentId is null) return RedirectToAction("Login", "Account");

        var user = await users.GetByIdAsync(studentId.Value);
        if (user is null) return RedirectToAction("Login", "Account");

        // GetByStudentAsync already includes Course + CourseProgress
        var myEnrollments = (await enrollments.GetByStudentAsync(studentId.Value))
            .Where(e => e.EnrollmentStatus == EnrollmentStatus.Active)
            .ToList();

        var cards = myEnrollments.Select(e => new CourseProgressCard
        {
            Enrollment = e,
            Progress = e.CourseProgress,
        }).ToList();

        var userBadges = (await badges.GetUserBadgesAsync(studentId.Value)).ToList();

        var weekAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6));
        var recentActivity = await db.DailyActivityLogs
            .AsNoTracking()
            .Where(d => d.UserId == studentId.Value && d.ActivityDate >= weekAgo)
            .OrderByDescending(d => d.ActivityDate)
            .ToListAsync();

        return View(new DashboardViewModel
        {
            Student = user,
            EnrolledCourses = cards,
            RecentBadges = userBadges.Take(3),
            BadgeCount = userBadges.Count,
            RecentActivity = recentActivity,
        });
    }

    private int? GetStudentId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
