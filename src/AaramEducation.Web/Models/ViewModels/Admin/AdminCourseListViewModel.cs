using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Admin;

public class AdminCourseRow
{
    public Course Course { get; set; } = null!;
    public int EnrollmentCount { get; set; }
}

public class AdminCourseListViewModel
{
    public IEnumerable<AdminCourseRow> Courses { get; set; } = [];
}
