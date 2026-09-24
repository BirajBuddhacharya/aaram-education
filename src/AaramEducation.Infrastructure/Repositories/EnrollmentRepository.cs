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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _db;
        public EnrollmentRepository(ApplicationDbContext db) { _db = db; }

        public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId) =>
            await _db.Enrollments.AsNoTracking()
                .Include(e => e.Course.CreatedBy)
                .Include(e => e.CourseProgress)
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.EnrollmentDate)
                .ToListAsync();

        public async Task<IEnumerable<Enrollment>> GetByCourseAsync(int courseId) =>
            await _db.Enrollments.AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.CourseProgress)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

        public Task<Enrollment?> GetAsync(int studentId, int courseId) =>
            _db.Enrollments.AsNoTracking()
                .Include(e => e.CourseProgress)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        public Task<bool> IsEnrolledAsync(int studentId, int courseId) =>
            _db.Enrollments.AnyAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId &&
                e.EnrollmentStatus == EnrollmentStatus.Active);

        public async Task<Enrollment> EnrollAsync(int studentId, int courseId)
        {
            var existing = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (existing is not null)
            {
                existing.EnrollmentStatus = EnrollmentStatus.Active;
                existing.EnrollmentDate = System.DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return existing;
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = System.DateTime.UtcNow,
                EnrollmentStatus = EnrollmentStatus.Active,
            };
            _db.Enrollments.Add(enrollment);
            await _db.SaveChangesAsync();

            _db.CourseProgresses.Add(new CourseProgress
            {
                EnrollmentId = enrollment.EnrollmentId,
                Status = ProgressStatus.NotStarted,
                LastAccessedAt = System.DateTime.UtcNow,
            });
            await _db.SaveChangesAsync();

            return enrollment;
        }

        public async Task DropAsync(int studentId, int courseId)
        {
            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment is not null)
            {
                enrollment.EnrollmentStatus = EnrollmentStatus.Dropped;
                await _db.SaveChangesAsync();
            }
        }
    }
}
