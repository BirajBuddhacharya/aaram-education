using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class ProgressRepository(ApplicationDbContext db) : IProgressRepository
{
    public Task<LessonProgress?> GetLessonProgressAsync(int studentId, int lessonId) =>
        db.LessonProgresses.AsNoTracking()
            .FirstOrDefaultAsync(lp => lp.StudentId == studentId && lp.LessonId == lessonId);

    public async Task<HashSet<int>> GetCompletedLessonIdsAsync(int studentId, IEnumerable<int> lessonIds)
    {
        var ids = lessonIds.ToList();
        var done = await db.LessonProgresses.AsNoTracking()
            .Where(lp => lp.StudentId == studentId
                      && ids.Contains(lp.LessonId)
                      && lp.Status == ProgressStatus.Completed)
            .Select(lp => lp.LessonId)
            .ToListAsync();
        return done.ToHashSet();
    }

    public async Task UpsertLessonProgressAsync(int studentId, int lessonId, ProgressStatus status, int? videoPositionSeconds = null)
    {
        var lp = await db.LessonProgresses
            .FirstOrDefaultAsync(x => x.StudentId == studentId && x.LessonId == lessonId);

        var now = DateTime.UtcNow;

        if (lp is null)
        {
            lp = new LessonProgress
            {
                StudentId = studentId,
                LessonId = lessonId,
                Status = status,
                VideoPositionSeconds = videoPositionSeconds,
                StartedAt = now,
                LastAccessedAt = now,
            };
            db.LessonProgresses.Add(lp);
        }
        else
        {
            if (lp.Status != ProgressStatus.Completed)
                lp.Status = status;

            if (videoPositionSeconds.HasValue)
                lp.VideoPositionSeconds = videoPositionSeconds;

            if (status == ProgressStatus.Completed && lp.CompletedAt is null)
                lp.CompletedAt = now;

            lp.LastAccessedAt = now;
        }

        await db.SaveChangesAsync();
    }

    public Task<ModuleProgress?> GetModuleProgressAsync(int studentId, int moduleId) =>
        db.ModuleProgresses.AsNoTracking()
            .FirstOrDefaultAsync(mp => mp.StudentId == studentId && mp.ModuleId == moduleId);

    public Task<CourseProgress?> GetCourseProgressAsync(int enrollmentId) =>
        db.CourseProgresses.AsNoTracking()
            .FirstOrDefaultAsync(cp => cp.EnrollmentId == enrollmentId);

    public async Task UpdateRollupsAsync(int studentId, int lessonId)
    {
        var lesson = await db.Lessons
            .Include(l => l.Module).ThenInclude(m => m.Lessons)
            .Include(l => l.Module).ThenInclude(m => m.Course).ThenInclude(c => c.Modules).ThenInclude(m => m.Lessons)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        if (lesson is null) return;

        var module = lesson.Module;
        var course = module.Course;
        var now = DateTime.UtcNow;

        // ── Module progress ────────────────────────────────────────────
        var completedInModule = await db.LessonProgresses
            .CountAsync(lp => lp.StudentId == studentId &&
                              lp.Status == ProgressStatus.Completed &&
                              module.Lessons.Select(l => l.LessonId).Contains(lp.LessonId));

        var totalInModule = module.Lessons.Count;

        var mp = await db.ModuleProgresses
            .FirstOrDefaultAsync(x => x.StudentId == studentId && x.ModuleId == module.ModuleId);

        if (mp is null)
        {
            mp = new ModuleProgress { StudentId = studentId, ModuleId = module.ModuleId, TotalLessons = totalInModule, LastAccessedAt = now };
            db.ModuleProgresses.Add(mp);
        }

        mp.LessonsCompleted = completedInModule;
        mp.TotalLessons = totalInModule;
        mp.LastAccessedAt = now;
        mp.Status = completedInModule == 0 ? ProgressStatus.NotStarted
                  : completedInModule == totalInModule ? ProgressStatus.Completed
                  : ProgressStatus.InProgress;
        if (mp.Status == ProgressStatus.Completed && mp.CompletedAt is null)
            mp.CompletedAt = now;

        await db.SaveChangesAsync();

        // ── Course progress ────────────────────────────────────────────
        var enrollment = await db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == course.CourseId);
        if (enrollment is null) return;

        var allLessonIds = course.Modules.SelectMany(m => m.Lessons).Select(l => l.LessonId).ToList();
        var allModuleIds = course.Modules.Select(m => m.ModuleId).ToList();
        var totalLessons = allLessonIds.Count;

        var completedLessons = await db.LessonProgresses
            .CountAsync(lp => lp.StudentId == studentId &&
                              lp.Status == ProgressStatus.Completed &&
                              allLessonIds.Contains(lp.LessonId));

        var completedModules = await db.ModuleProgresses
            .CountAsync(mpr => mpr.StudentId == studentId &&
                               mpr.Status == ProgressStatus.Completed &&
                               allModuleIds.Contains(mpr.ModuleId));

        var cp = await db.CourseProgresses
            .FirstOrDefaultAsync(x => x.EnrollmentId == enrollment.EnrollmentId);

        if (cp is null) return;

        cp.LessonsCompleted = completedLessons;
        cp.ModulesCompleted = completedModules;
        cp.PercentComplete = totalLessons > 0 ? (float)completedLessons / totalLessons * 100f : 0f;
        cp.LastAccessedAt = now;
        cp.Status = completedLessons == 0 ? ProgressStatus.NotStarted
                  : completedLessons == totalLessons ? ProgressStatus.Completed
                  : ProgressStatus.InProgress;
        if (cp.Status == ProgressStatus.Completed && cp.CompletedAt is null)
            cp.CompletedAt = now;

        await db.SaveChangesAsync();
    }

    public async Task<DailyActivityLog> UpsertDailyLogAsync(int userId, Action<DailyActivityLog> update)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var log = await db.DailyActivityLogs
            .FirstOrDefaultAsync(d => d.UserId == userId && d.ActivityDate == today);

        if (log is null)
        {
            log = new DailyActivityLog { UserId = userId, ActivityDate = today };
            db.DailyActivityLogs.Add(log);
        }

        update(log);
        await db.SaveChangesAsync();
        return log;
    }
}
