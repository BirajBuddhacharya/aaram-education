using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

public class CourseProgress
{
    public int CourseProgressId { get; set; }
    public int EnrollmentId { get; set; }
    public ProgressStatus Status { get; set; }
    public int LessonsCompleted { get; set; }
    public int ModulesCompleted { get; set; }
    public float PercentComplete { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime LastAccessedAt { get; set; }

    public Enrollment Enrollment { get; set; } = null!;
}
