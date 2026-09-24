using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }

    public User Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public CourseProgress? CourseProgress { get; set; }
}
