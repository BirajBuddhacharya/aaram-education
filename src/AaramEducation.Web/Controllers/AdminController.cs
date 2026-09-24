using System.Security.Claims;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(
    IUserRepository users,
    ICourseRepository courses,
    IGuestbookRepository guestbook,
    ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Dashboard()
    {
        var allUsers = (await users.GetAllAsync()).ToList();
        var allCourses = await db.Courses.AsNoTracking().ToListAsync();
        var enrollCount = await db.Enrollments.CountAsync();
        var pendingCount = (await guestbook.GetPendingAsync()).Count();

        return View(new AdminDashboardViewModel
        {
            TotalUsers = allUsers.Count,
            TotalStudents = allUsers.Count(u => u.Role == UserRole.Student),
            TotalTutors = allUsers.Count(u => u.Role == UserRole.Tutor),
            TotalCourses = allCourses.Count,
            PublishedCourses = allCourses.Count(c => c.IsPublished),
            TotalEnrollments = enrollCount,
            PendingGuestbookEntries = pendingCount,
        });
    }

    // ── Users ─────────────────────────────────────────────────────────────
    public async Task<IActionResult> Users()
    {
        var all = await users.GetAllAsync();
        return View(new AdminUserListViewModel { Users = all });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(int userId, UserRole role)
    {
        var user = await users.GetByIdAsync(userId);
        if (user is null) return NotFound();

        if (user.UserId == GetAdminId())
        {
            TempData["Error"] = "Cannot change your own role.";
            return RedirectToAction("Users");
        }

        user.Role = role;
        user.UpdatedAt = DateTime.UtcNow;
        await users.UpdateAsync(user);
        TempData["Success"] = $"Role updated for {user.Email}.";
        return RedirectToAction("Users");
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        if (userId == GetAdminId())
        {
            TempData["Error"] = "Cannot delete your own account.";
            return RedirectToAction("Users");
        }
        await users.DeleteAsync(userId);
        TempData["Success"] = "User deleted.";
        return RedirectToAction("Users");
    }

    // ── Courses ───────────────────────────────────────────────────────────
    public async Task<IActionResult> Courses()
    {
        var all = await db.Courses.AsNoTracking()
            .Include(c => c.CreatedBy)
            .Include(c => c.Enrollments)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var rows = all.Select(c => new AdminCourseRow
        {
            Course = c,
            EnrollmentCount = c.Enrollments.Count,
        });
        return View(new AdminCourseListViewModel { Courses = rows });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(int courseId)
    {
        var course = await courses.GetByIdAsync(courseId);
        if (course is null) return NotFound();
        await courses.PublishAsync(courseId, !course.IsPublished);
        return RedirectToAction("Courses");
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCourse(int courseId)
    {
        await courses.DeleteAsync(courseId);
        return RedirectToAction("Courses");
    }

    // ── Guestbook ─────────────────────────────────────────────────────────
    public async Task<IActionResult> Guestbook()
    {
        var pending = await guestbook.GetPendingAsync();
        return View(new AdminGuestbookViewModel { Pending = pending });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveEntry(int entryId)
    {
        await guestbook.ModerateAsync(entryId, ModerationStatus.Approved, GetAdminId()!.Value);
        return RedirectToAction("Guestbook");
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectEntry(int entryId)
    {
        await guestbook.ModerateAsync(entryId, ModerationStatus.Rejected, GetAdminId()!.Value);
        return RedirectToAction("Guestbook");
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEntry(int entryId)
    {
        await guestbook.DeleteAsync(entryId);
        return RedirectToAction("Guestbook");
    }

    private int? GetAdminId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
