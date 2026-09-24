using System.Security.Claims;
using AaramEducation.Core.Interfaces;
using AaramEducation.Web.Models.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

public class CoursesController(ICourseRepository courses, IEnrollmentRepository enrollments) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? subject, string? difficulty)
    {
        var all = await courses.GetAllPublishedAsync(subject, difficulty);
        var subjects = (await courses.GetAllPublishedAsync()).Select(c => c.Subject).Distinct().OrderBy(s => s);

        var enrolled = new HashSet<int>();
        if (GetCurrentUserId() is int uid)
        {
            var myEnrollments = await enrollments.GetByStudentAsync(uid);
            enrolled = myEnrollments.Select(e => e.CourseId).ToHashSet();
        }

        return View(new CourseListViewModel
        {
            Courses = all,
            SelectedSubject = subject,
            SelectedDifficulty = difficulty,
            Subjects = subjects,
            EnrolledCourseIds = enrolled,
        });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var course = await courses.GetWithModulesAndLessonsAsync(id);
        if (course is null) return NotFound();

        var vm = new CourseDetailsViewModel { Course = course };

        if (GetCurrentUserId() is int uid)
        {
            var enrollment = await enrollments.GetAsync(uid, id);
            if (enrollment is not null)
            {
                vm.IsEnrolled = true;
                vm.EnrollmentStatus = enrollment.EnrollmentStatus;
                vm.PercentComplete = enrollment.CourseProgress?.PercentComplete ?? 0f;
            }
        }

        return View(vm);
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var uid = GetCurrentUserId();
        if (uid is null) return RedirectToAction("Login", "Account");

        await enrollments.EnrollAsync(uid.Value, courseId);
        return RedirectToAction("Details", new { id = courseId });
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Drop(int courseId)
    {
        var uid = GetCurrentUserId();
        if (uid is null) return RedirectToAction("Login", "Account");

        await enrollments.DropAsync(uid.Value, courseId);
        return RedirectToAction("MyEnrollments");
    }

    [Authorize, HttpGet]
    public async Task<IActionResult> MyEnrollments()
    {
        var uid = GetCurrentUserId();
        if (uid is null) return RedirectToAction("Login", "Account");

        var myEnrollments = await enrollments.GetByStudentAsync(uid.Value);
        return View(new EnrollmentListViewModel { Enrollments = myEnrollments });
    }

    private int? GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out var id) ? id : null;
    }
}
