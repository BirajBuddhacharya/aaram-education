using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;

namespace AaramEducation.Web.Models.ViewModels.Courses;

public class CourseDetailsViewModel
{
    public Course Course { get; set; } = null!;
    public bool IsEnrolled { get; set; }
    public EnrollmentStatus? EnrollmentStatus { get; set; }
    public float PercentComplete { get; set; }
}
