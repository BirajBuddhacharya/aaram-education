namespace AaramEducation.Core.Entities;

public class StudyNote
{
    public int NoteId { get; set; }
    public int LessonId { get; set; }
    public string NoteTitle { get; set; } = string.Empty;
    public string? NoteContent { get; set; }
    public string? FileUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public Lesson Lesson { get; set; } = null!;
}
