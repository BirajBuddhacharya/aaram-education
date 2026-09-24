using System.Security.Claims;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.Models.ViewModels.Tutor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Web.Controllers;

[Authorize(Roles = "Tutor")]
public class TutorController(
    ICourseRepository courses,
    ILessonRepository lessons,
    ApplicationDbContext db,
    IWebHostEnvironment env) : Controller
{
    public async Task<IActionResult> Dashboard()
    {
        var tid = GetTutorId()!.Value;
        var myCourses = (await courses.GetByTutorAsync(tid)).ToList();
        var rows = myCourses.Select(c => new TutorCourseRow
        {
            Course = c,
            EnrollmentCount = c.Enrollments.Count,
        });
        return View(new TutorDashboardViewModel { Courses = rows });
    }

    public IActionResult CreateCourse() => View("CourseForm", new CourseFormViewModel());

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCourse(CourseFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("CourseForm", vm);
        var course = new Course
        {
            CreatedByUserId = GetTutorId()!.Value,
            CourseName = vm.CourseName,
            CourseDescription = vm.CourseDescription,
            Subject = vm.Subject,
            DifficultyLevel = vm.DifficultyLevel,
            IsPublished = vm.IsPublished,
            CreatedAt = DateTime.UtcNow,
        };
        await courses.CreateAsync(course);
        return RedirectToAction("Dashboard");
    }

    public async Task<IActionResult> EditCourse(int id)
    {
        var course = await courses.GetByIdAsync(id);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        return View("CourseForm", new CourseFormViewModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            CourseDescription = course.CourseDescription,
            Subject = course.Subject,
            DifficultyLevel = course.DifficultyLevel,
            IsPublished = course.IsPublished,
        });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCourse(CourseFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("CourseForm", vm);
        var course = await courses.GetByIdAsync(vm.CourseId);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        course.CourseName = vm.CourseName;
        course.CourseDescription = vm.CourseDescription;
        course.Subject = vm.Subject;
        course.DifficultyLevel = vm.DifficultyLevel;
        course.IsPublished = vm.IsPublished;
        await courses.UpdateAsync(course);
        return RedirectToAction("Dashboard");
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await courses.GetByIdAsync(id);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        await courses.DeleteAsync(id);
        return RedirectToAction("Dashboard");
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(int id)
    {
        var course = await courses.GetByIdAsync(id);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        await courses.PublishAsync(id, !course.IsPublished);
        return RedirectToAction("ManageCourse", new { id });
    }

    public async Task<IActionResult> ManageCourse(int id)
    {
        var course = await courses.GetWithModulesAndLessonsAsync(id);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        return View(course);
    }

    public async Task<IActionResult> CreateModule(int courseId)
    {
        var course = await courses.GetByIdAsync(courseId);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        return View("ModuleForm", new ModuleFormViewModel { CourseId = courseId });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateModule(ModuleFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("ModuleForm", vm);
        var course = await courses.GetByIdAsync(vm.CourseId);
        if (course is null || course.CreatedByUserId != GetTutorId()) return Forbid();
        db.Modules.Add(new AaramEducation.Core.Entities.Module
        {
            CourseId = vm.CourseId,
            ModuleName = vm.ModuleName,
            ModuleDescription = vm.ModuleDescription,
            SequenceOrder = vm.SequenceOrder,
            CreatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
        return RedirectToAction("ManageCourse", new { id = vm.CourseId });
    }

    public async Task<IActionResult> EditModule(int id)
    {
        var mod = await db.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.ModuleId == id);
        if (mod is null || mod.Course.CreatedByUserId != GetTutorId()) return Forbid();
        return View("ModuleForm", new ModuleFormViewModel
        {
            ModuleId = mod.ModuleId,
            CourseId = mod.CourseId,
            ModuleName = mod.ModuleName,
            ModuleDescription = mod.ModuleDescription,
            SequenceOrder = mod.SequenceOrder,
        });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> EditModule(ModuleFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("ModuleForm", vm);
        var mod = await db.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.ModuleId == vm.ModuleId);
        if (mod is null || mod.Course.CreatedByUserId != GetTutorId()) return Forbid();
        mod.ModuleName = vm.ModuleName;
        mod.ModuleDescription = vm.ModuleDescription;
        mod.SequenceOrder = vm.SequenceOrder;
        await db.SaveChangesAsync();
        return RedirectToAction("ManageCourse", new { id = mod.CourseId });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteModule(int id)
    {
        var mod = await db.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.ModuleId == id);
        if (mod is null || mod.Course.CreatedByUserId != GetTutorId()) return Forbid();
        var courseId = mod.CourseId;
        db.Modules.Remove(mod);
        await db.SaveChangesAsync();
        return RedirectToAction("ManageCourse", new { id = courseId });
    }

    public async Task<IActionResult> CreateLesson(int moduleId)
    {
        var mod = await db.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.ModuleId == moduleId);
        if (mod is null || mod.Course.CreatedByUserId != GetTutorId()) return Forbid();
        return View("LessonForm", new LessonFormViewModel { ModuleId = moduleId });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLesson(LessonFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("LessonForm", vm);
        var mod = await db.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.ModuleId == vm.ModuleId);
        if (mod is null || mod.Course.CreatedByUserId != GetTutorId()) return Forbid();

        var lesson = new Lesson
        {
            ModuleId = vm.ModuleId,
            LessonTitle = vm.LessonTitle,
            LessonDescription = vm.LessonDescription,
            SequenceOrder = vm.SequenceOrder,
            IsFreeSample = vm.IsFreeSample,
            CreatedAt = DateTime.UtcNow,
        };
        var created = await lessons.CreateAsync(lesson);

        if (!string.IsNullOrWhiteSpace(vm.VideoUrl))
            await lessons.SaveVideoAsync(new Video
            {
                LessonId = created.LessonId,
                VideoTitle = vm.VideoTitle ?? vm.LessonTitle,
                VideoUrl = vm.VideoUrl,
                DurationSeconds = vm.DurationSeconds,
                UploadedAt = DateTime.UtcNow,
            });

        if (!string.IsNullOrWhiteSpace(vm.NoteTitle))
        {
            string? fileUrl = null;
            if (vm.NoteFile is { Length: > 0 })
                fileUrl = await SaveNoteFileAsync(vm.NoteFile, created.LessonId);

            await lessons.SaveStudyNoteAsync(new StudyNote
            {
                LessonId = created.LessonId,
                NoteTitle = vm.NoteTitle,
                NoteContent = vm.NoteContent,
                FileUrl = fileUrl,
                CreatedAt = DateTime.UtcNow,
            });
        }

        return RedirectToAction("ManageCourse", new { id = mod.CourseId });
    }

    public async Task<IActionResult> EditLesson(int id)
    {
        var lesson = await lessons.GetWithContentAsync(id);
        if (lesson is null || lesson.Module.Course.CreatedByUserId != GetTutorId()) return Forbid();
        var vid = lesson.Videos.FirstOrDefault();
        var note = lesson.StudyNotes.FirstOrDefault();
        return View("LessonForm", new LessonFormViewModel
        {
            LessonId = lesson.LessonId,
            ModuleId = lesson.ModuleId,
            LessonTitle = lesson.LessonTitle,
            LessonDescription = lesson.LessonDescription,
            SequenceOrder = lesson.SequenceOrder,
            IsFreeSample = lesson.IsFreeSample,
            VideoTitle = vid?.VideoTitle,
            VideoUrl = vid?.VideoUrl,
            DurationSeconds = vid?.DurationSeconds ?? 0,
            NoteTitle = note?.NoteTitle,
            NoteContent = note?.NoteContent,
        });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLesson(LessonFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("LessonForm", vm);
        var lesson = await lessons.GetWithContentAsync(vm.LessonId);
        if (lesson is null || lesson.Module.Course.CreatedByUserId != GetTutorId()) return Forbid();

        lesson.LessonTitle = vm.LessonTitle;
        lesson.LessonDescription = vm.LessonDescription;
        lesson.SequenceOrder = vm.SequenceOrder;
        lesson.IsFreeSample = vm.IsFreeSample;
        await lessons.UpdateAsync(lesson);

        var vid = lesson.Videos.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(vm.VideoUrl))
        {
            if (vid is null)
                await lessons.SaveVideoAsync(new Video
                {
                    LessonId = vm.LessonId,
                    VideoTitle = vm.VideoTitle ?? vm.LessonTitle,
                    VideoUrl = vm.VideoUrl,
                    DurationSeconds = vm.DurationSeconds,
                    UploadedAt = DateTime.UtcNow,
                });
            else
            {
                vid.VideoTitle = vm.VideoTitle ?? vm.LessonTitle;
                vid.VideoUrl = vm.VideoUrl;
                vid.DurationSeconds = vm.DurationSeconds;
                db.Videos.Update(vid);
                await db.SaveChangesAsync();
            }
        }

        return RedirectToAction("ManageCourse", new { id = lesson.Module.CourseId });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var lesson = await lessons.GetWithContentAsync(id);
        if (lesson is null || lesson.Module.Course.CreatedByUserId != GetTutorId()) return Forbid();
        var courseId = lesson.Module.CourseId;
        await lessons.DeleteAsync(id);
        return RedirectToAction("ManageCourse", new { id = courseId });
    }

    private int? GetTutorId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }

    private async Task<string> SaveNoteFileAsync(IFormFile file, int lessonId)
    {
        var ext = Path.GetExtension(file.FileName);
        var name = $"note_{lessonId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{ext}";
        var path = Path.Combine(env.WebRootPath, "uploads", "notes", name);
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
        return $"/uploads/notes/{name}";
    }
}
