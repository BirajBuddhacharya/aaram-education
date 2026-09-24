using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;

namespace AaramEducation.Web.Models.ViewModels.Lessons;

public class LessonViewModel
{
    public Lesson Lesson { get; set; } = null!;
    public Video? Video { get; set; }
    public IEnumerable<StudyNote> StudyNotes { get; set; } = [];
    public IEnumerable<Quiz> Quizzes { get; set; } = [];
    public ProgressStatus LessonStatus { get; set; }
    public int ResumePositionSeconds { get; set; }
    public int? PrevLessonId { get; set; }
    public int? NextLessonId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string TutorName { get; set; } = string.Empty;
    public IReadOnlyList<Lesson> Siblings { get; set; } = [];
    public HashSet<int> CompletedLessonIds { get; set; } = [];
}
