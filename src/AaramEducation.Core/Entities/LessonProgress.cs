using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

public class LessonProgress
{
    public int LessonProgressId { get; set; }
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public ProgressStatus Status { get; set; }
    public int? VideoPositionSeconds { get; set; }
    public int TimeSpentSeconds { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime LastAccessedAt { get; set; }

    public User Student { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}
