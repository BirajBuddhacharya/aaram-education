namespace AaramEducation.Core.Entities;

public class Module
{
    public int ModuleId { get; set; }
    public int CourseId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string? ModuleDescription { get; set; }
    public int SequenceOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    public Course Course { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = [];
    public ICollection<ModuleProgress> ModuleProgresses { get; set; } = [];
}
