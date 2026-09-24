using System.Collections.Generic;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;

namespace AaramEducation.Core.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllPublishedAsync(string? subject = null, string? difficulty = null);
        Task<IEnumerable<Course>> GetByTutorAsync(int tutorUserId);
        Task<Course?> GetByIdAsync(int courseId);
        Task<Course?> GetWithModulesAndLessonsAsync(int courseId);
        Task<Course> CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int courseId);
        Task PublishAsync(int courseId, bool publish);
    }
}
