using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Tutor;

public class TutorCourseRow
{
    public Course Course { get; set; } = null!;
    public int EnrollmentCount { get; set; }
}

public class TutorDashboardViewModel
{
    public IEnumerable<TutorCourseRow> Courses { get; set; } = [];
}
