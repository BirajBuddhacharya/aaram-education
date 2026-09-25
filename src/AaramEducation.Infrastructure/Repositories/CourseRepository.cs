using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;

namespace AaramEducation.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseRepository(ApplicationDbContext db) { _db = db; }

        public async Task<IEnumerable<Course>> GetAllPublishedAsync(string? subject = null, string? difficulty = null)
        {
            var q = _db.Courses.AsNoTracking()
                .Include(c => c.CreatedBy)
                .Include(c => c.Modules.Select(m => m.Lessons))
                .Where(c => c.IsPublished);

            if (!string.IsNullOrEmpty(subject))
                q = q.Where(c => c.Subject == subject);
            if (!string.IsNullOrEmpty(difficulty))
                q = q.Where(c => c.DifficultyLevel == difficulty);

            return await q.OrderBy(c => c.CourseName).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Course>> GetByTutorAsync(int tutorUserId)
        {
            var courses = await _db.Courses.AsNoTracking()
                .Include(c => c.Modules.Select(m => m.Lessons))
                .Include(c => c.Enrollments)
                .Where(c => c.CreatedByUserId == tutorUserId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync().ConfigureAwait(false);
            return courses;
        }

        public Task<Course?> GetByIdAsync(int courseId) =>
            _db.Courses.AsNoTracking()
                .Include(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

        public async Task<Course?> GetWithModulesAndLessonsAsync(int courseId)
        {
            var course = await _db.Courses.AsNoTracking()
                .Include(c => c.CreatedBy)
                .Include(c => c.Modules.Select(m => m.Lessons))
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.CourseId == courseId).ConfigureAwait(false);

            if (course != null)
            {
                // EF6 can't sort inside Include — sort in memory
                foreach (var mod in course.Modules)
                    mod.Lessons = mod.Lessons.OrderBy(l => l.SequenceOrder).ToList();
            }
            return course;
        }

        public async Task<Course> CreateAsync(Course course)
        {
            _db.Courses.Add(course);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return course;
        }

        public async Task UpdateAsync(Course course)
        {
            _db.Entry(course).State = EntityState.Modified;
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int courseId)
        {
            var course = await _db.Courses.FindAsync(courseId).ConfigureAwait(false);
            if (course is not null)
            {
                _db.Courses.Remove(course);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task PublishAsync(int courseId, bool publish)
        {
            var course = await _db.Courses.FindAsync(courseId).ConfigureAwait(false);
            if (course is not null)
            {
                course.IsPublished = publish;
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
