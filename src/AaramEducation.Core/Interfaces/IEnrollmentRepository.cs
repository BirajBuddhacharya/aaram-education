using System.Collections.Generic;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;

namespace AaramEducation.Core.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId);
        Task<IEnumerable<Enrollment>> GetByCourseAsync(int courseId);
        Task<Enrollment?> GetAsync(int studentId, int courseId);
        Task<bool> IsEnrolledAsync(int studentId, int courseId);
        Task<Enrollment> EnrollAsync(int studentId, int courseId);
        Task DropAsync(int studentId, int courseId);
    }
}
