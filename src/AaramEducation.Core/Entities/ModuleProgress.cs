using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

public class ModuleProgress
{
    public int ModuleProgressId { get; set; }
    public int StudentId { get; set; }
    public int ModuleId { get; set; }
    public ProgressStatus Status { get; set; }
    public int LessonsCompleted { get; set; }
    public int TotalLessons { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime LastAccessedAt { get; set; }

    public User Student { get; set; } = null!;
    public Module Module { get; set; } = null!;
}
