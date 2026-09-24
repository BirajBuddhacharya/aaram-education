using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Courses;

public class EnrollmentListViewModel
{
    public IEnumerable<Enrollment> Enrollments { get; set; } = [];
}
