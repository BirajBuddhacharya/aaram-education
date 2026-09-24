using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class CourseRepository(ApplicationDbContext db) : ICourseRepository
{
    public async Task<IEnumerable<Course>> GetAllPublishedAsync(string? subject = null, string? difficulty = null)
    {
        var q = db.Courses.AsNoTracking()
            .Include(c => c.CreatedBy)
            .Include(c => c.Modules).ThenInclude(m => m.Lessons)
            .Where(c => c.IsPublished);

        if (!string.IsNullOrEmpty(subject))
            q = q.Where(c => c.Subject == subject);

        if (!string.IsNullOrEmpty(difficulty))
            q = q.Where(c => c.DifficultyLevel == difficulty);

        return await q.OrderBy(c => c.CourseName).ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByTutorAsync(int tutorUserId) =>
        await db.Courses.AsNoTracking()
            .Include(c => c.Modules).ThenInclude(m => m.Lessons)
            .Where(c => c.CreatedByUserId == tutorUserId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

    public Task<Course?> GetByIdAsync(int courseId) =>
        db.Courses.AsNoTracking()
            .Include(c => c.CreatedBy)
            .FirstOrDefaultAsync(c => c.CourseId == courseId);

    public Task<Course?> GetWithModulesAndLessonsAsync(int courseId) =>
        db.Courses.AsNoTracking()
            .Include(c => c.CreatedBy)
            .Include(c => c.Modules.OrderBy(m => m.SequenceOrder))
                .ThenInclude(m => m.Lessons.OrderBy(l => l.SequenceOrder))
            .FirstOrDefaultAsync(c => c.CourseId == courseId);

    public async Task<Course> CreateAsync(Course course)
    {
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return course;
    }

    public async Task UpdateAsync(Course course)
    {
        db.Courses.Update(course);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int courseId)
    {
        var course = await db.Courses.FindAsync(courseId);
        if (course is not null)
        {
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
        }
    }

    public async Task PublishAsync(int courseId, bool publish)
    {
        var course = await db.Courses.FindAsync(courseId);
        if (course is not null)
        {
            course.IsPublished = publish;
            await db.SaveChangesAsync();
        }
    }
}
