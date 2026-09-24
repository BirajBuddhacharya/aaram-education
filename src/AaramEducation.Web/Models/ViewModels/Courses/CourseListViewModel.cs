using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Courses;

public class CourseListViewModel
{
    public IEnumerable<Course> Courses { get; set; } = [];
    public string? SelectedSubject { get; set; }
    public string? SelectedDifficulty { get; set; }
    public IEnumerable<string> Subjects { get; set; } = [];
    public HashSet<int> EnrolledCourseIds { get; set; } = [];
}
