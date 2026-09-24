using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class EnrollmentRepository(ApplicationDbContext db) : IEnrollmentRepository
{
    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId) =>
        await db.Enrollments.AsNoTracking()
            .Include(e => e.Course).ThenInclude(c => c.CreatedBy)
            .Include(e => e.CourseProgress)
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByCourseAsync(int courseId) =>
        await db.Enrollments.AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.CourseProgress)
            .Where(e => e.CourseId == courseId)
            .ToListAsync();

    public Task<Enrollment?> GetAsync(int studentId, int courseId) =>
        db.Enrollments.AsNoTracking()
            .Include(e => e.CourseProgress)
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    public Task<bool> IsEnrolledAsync(int studentId, int courseId) =>
        db.Enrollments.AnyAsync(e =>
            e.StudentId == studentId &&
            e.CourseId == courseId &&
            e.EnrollmentStatus == EnrollmentStatus.Active);

    public async Task<Enrollment> EnrollAsync(int studentId, int courseId)
    {
        // Re-activate if previously dropped
        var existing = await db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (existing is not null)
        {
            existing.EnrollmentStatus = EnrollmentStatus.Active;
            existing.EnrollmentDate = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return existing;
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.UtcNow,
            EnrollmentStatus = EnrollmentStatus.Active,
        };
        db.Enrollments.Add(enrollment);
        await db.SaveChangesAsync();

        db.CourseProgresses.Add(new CourseProgress
        {
            EnrollmentId = enrollment.EnrollmentId,
            Status = ProgressStatus.NotStarted,
            LastAccessedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();

        return enrollment;
    }

    public async Task DropAsync(int studentId, int courseId)
    {
        var enrollment = await db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (enrollment is not null)
        {
            enrollment.EnrollmentStatus = EnrollmentStatus.Dropped;
            await db.SaveChangesAsync();
        }
    }
}
