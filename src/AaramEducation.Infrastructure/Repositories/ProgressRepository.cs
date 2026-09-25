using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;

namespace AaramEducation.Infrastructure.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly ApplicationDbContext _db;
        public ProgressRepository(ApplicationDbContext db) { _db = db; }

        public Task<LessonProgress?> GetLessonProgressAsync(int studentId, int lessonId) =>
            _db.LessonProgresses.AsNoTracking()
                .FirstOrDefaultAsync(lp => lp.StudentId == studentId && lp.LessonId == lessonId);

        public async Task<HashSet<int>> GetCompletedLessonIdsAsync(int studentId, IEnumerable<int> lessonIds)
        {
            var ids = lessonIds.ToList();
            var done = await _db.LessonProgresses.AsNoTracking()
                .Where(lp => lp.StudentId == studentId
                          && ids.Contains(lp.LessonId)
                          && lp.Status == ProgressStatus.Completed)
                .Select(lp => lp.LessonId)
                .ToListAsync().ConfigureAwait(false);
            return new HashSet<int>(done);
        }

        public async Task UpsertLessonProgressAsync(int studentId, int lessonId, ProgressStatus status, int? videoPositionSeconds = null)
        {
            var lp = await _db.LessonProgresses
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.LessonId == lessonId).ConfigureAwait(false);

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
                _db.LessonProgresses.Add(lp);
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

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<ModuleProgress?> GetModuleProgressAsync(int studentId, int moduleId) =>
            _db.ModuleProgresses.AsNoTracking()
                .FirstOrDefaultAsync(mp => mp.StudentId == studentId && mp.ModuleId == moduleId);

        public Task<CourseProgress?> GetCourseProgressAsync(int enrollmentId) =>
            _db.CourseProgresses.AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.EnrollmentId == enrollmentId);

        public async Task UpdateRollupsAsync(int studentId, int lessonId)
        {
            // EF6: nested include via .Select() for collection-in-collection
            var lesson = await _db.Lessons
                .Include(l => l.Module.Lessons)
                .Include(l => l.Module.Course.Modules.Select(m => m.Lessons))
                .FirstOrDefaultAsync(l => l.LessonId == lessonId).ConfigureAwait(false);

            if (lesson is null) return;

            var module = lesson.Module;
            var course = module.Course;
            var now = DateTime.UtcNow;

            var moduleLessonIds = module.Lessons.Select(l => l.LessonId).ToList();
            var completedInModule = await _db.LessonProgresses
                .CountAsync(lp => lp.StudentId == studentId &&
                                  lp.Status == ProgressStatus.Completed &&
                                  moduleLessonIds.Contains(lp.LessonId)).ConfigureAwait(false);

            var totalInModule = module.Lessons.Count;

            var mp = await _db.ModuleProgresses
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.ModuleId == module.ModuleId).ConfigureAwait(false);

            if (mp is null)
            {
                mp = new ModuleProgress { StudentId = studentId, ModuleId = module.ModuleId, TotalLessons = totalInModule, LastAccessedAt = now };
                _db.ModuleProgresses.Add(mp);
            }

            mp.LessonsCompleted = completedInModule;
            mp.TotalLessons = totalInModule;
            mp.LastAccessedAt = now;
            mp.Status = completedInModule == 0 ? ProgressStatus.NotStarted
                      : completedInModule == totalInModule ? ProgressStatus.Completed
                      : ProgressStatus.InProgress;
            if (mp.Status == ProgressStatus.Completed && mp.CompletedAt is null)
                mp.CompletedAt = now;

            await _db.SaveChangesAsync().ConfigureAwait(false);

            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == course.CourseId).ConfigureAwait(false);
            if (enrollment is null) return;

            var allLessonIds = course.Modules.SelectMany(m => m.Lessons).Select(l => l.LessonId).ToList();
            var allModuleIds = course.Modules.Select(m => m.ModuleId).ToList();
            var totalLessons = allLessonIds.Count;

            var completedLessons = await _db.LessonProgresses
                .CountAsync(lp => lp.StudentId == studentId &&
                                  lp.Status == ProgressStatus.Completed &&
                                  allLessonIds.Contains(lp.LessonId)).ConfigureAwait(false);

            var completedModules = await _db.ModuleProgresses
                .CountAsync(mpr => mpr.StudentId == studentId &&
                                   mpr.Status == ProgressStatus.Completed &&
                                   allModuleIds.Contains(mpr.ModuleId)).ConfigureAwait(false);

            var cp = await _db.CourseProgresses
                .FirstOrDefaultAsync(x => x.EnrollmentId == enrollment.EnrollmentId).ConfigureAwait(false);

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

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<DailyActivityLog> UpsertDailyLogAsync(int userId, Action<DailyActivityLog> update)
        {
            var today = DateTime.UtcNow.Date;

            var log = await _db.DailyActivityLogs
                .FirstOrDefaultAsync(d => d.UserId == userId && d.ActivityDate == today).ConfigureAwait(false);

            if (log is null)
            {
                log = new DailyActivityLog { UserId = userId, ActivityDate = today };
                _db.DailyActivityLogs.Add(log);
            }

            update(log);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return log;
        }
    }
}
