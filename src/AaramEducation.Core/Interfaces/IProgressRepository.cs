using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Interfaces;

public interface IProgressRepository
{
    Task<LessonProgress?> GetLessonProgressAsync(int studentId, int lessonId);
    Task<HashSet<int>> GetCompletedLessonIdsAsync(int studentId, IEnumerable<int> lessonIds);
    Task UpsertLessonProgressAsync(int studentId, int lessonId, ProgressStatus status, int? videoPositionSeconds = null);
    Task<ModuleProgress?> GetModuleProgressAsync(int studentId, int moduleId);
    Task<CourseProgress?> GetCourseProgressAsync(int enrollmentId);
    Task UpdateRollupsAsync(int studentId, int lessonId);   // cascades lesson→module→course
    Task<DailyActivityLog> UpsertDailyLogAsync(int userId, Action<DailyActivityLog> update);
}
