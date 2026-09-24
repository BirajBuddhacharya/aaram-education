using System.Security.Claims;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Web.Models.ViewModels.Lessons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

public class LessonsController(
    ILessonRepository lessons,
    IProgressRepository progress,
    IEnrollmentRepository enrollments,
    IBadgeRepository badges) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Show(int id)
    {
        var lesson = await lessons.GetWithContentAsync(id);
        if (lesson is null) return NotFound();

        var course = lesson.Module.Course;
        int? studentId = GetStudentId();
        bool enrolled = false;

        if (studentId.HasValue)
        {
            var enrollment = await enrollments.GetAsync(studentId.Value, course.CourseId);
            enrolled = enrollment is { EnrollmentStatus: EnrollmentStatus.Active };
        }

        if (!lesson.IsFreeSample && !enrolled)
            return RedirectToAction("Details", "Courses", new { id = course.CourseId });

        LessonProgress? lp = null;
        if (studentId.HasValue)
        {
            lp = await progress.GetLessonProgressAsync(studentId.Value, id);
            if (enrolled && (lp is null || lp.Status == ProgressStatus.NotStarted))
                await progress.UpsertLessonProgressAsync(studentId.Value, id, ProgressStatus.InProgress);
        }

        var siblings = (await lessons.GetByModuleAsync(lesson.ModuleId)).ToList();
        var idx = siblings.FindIndex(l => l.LessonId == id);

        return View(new LessonViewModel
        {
            Lesson = lesson,
            Video = lesson.Videos.FirstOrDefault(),
            StudyNotes = lesson.StudyNotes,
            Quizzes = lesson.Quizzes,
            LessonStatus = lp?.Status ?? ProgressStatus.NotStarted,
            ResumePositionSeconds = lp?.VideoPositionSeconds ?? 0,
            PrevLessonId = idx > 0 ? siblings[idx - 1].LessonId : null,
            NextLessonId = idx >= 0 && idx < siblings.Count - 1 ? siblings[idx + 1].LessonId : null,
            CourseName = course.CourseName,
            CourseId = course.CourseId,
            ModuleName = lesson.Module.ModuleName,
        });
    }

    [Authorize] [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProgress([FromBody] SaveProgressRequest req)
    {
        var studentId = GetStudentId();
        if (!studentId.HasValue) return Unauthorized();

        await progress.UpsertLessonProgressAsync(studentId.Value, req.LessonId, ProgressStatus.InProgress, req.PositionSeconds);
        return Ok();
    }

    [Authorize] [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int lessonId)
    {
        var studentId = GetStudentId();
        if (!studentId.HasValue) return RedirectToAction("Login", "Account");

        await progress.UpsertLessonProgressAsync(studentId.Value, lessonId, ProgressStatus.Completed);
        await progress.UpdateRollupsAsync(studentId.Value, lessonId);
        await progress.UpsertDailyLogAsync(studentId.Value, log => log.LessonsCompleted++);
        await badges.CheckAndAwardAsync(studentId.Value);

        var lesson = await lessons.GetByIdAsync(lessonId);
        if (lesson is not null)
        {
            var siblings = (await lessons.GetByModuleAsync(lesson.ModuleId)).ToList();
            var idx = siblings.FindIndex(l => l.LessonId == lessonId);
            if (idx >= 0 && idx < siblings.Count - 1)
                return RedirectToAction("Show", new { id = siblings[idx + 1].LessonId });

            var full = await lessons.GetWithContentAsync(lessonId);
            if (full is not null)
                return RedirectToAction("Details", "Courses", new { id = full.Module.Course.CourseId });
        }

        return RedirectToAction("MyEnrollments", "Courses");
    }

    private int? GetStudentId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}

public record SaveProgressRequest(int LessonId, int PositionSeconds);
