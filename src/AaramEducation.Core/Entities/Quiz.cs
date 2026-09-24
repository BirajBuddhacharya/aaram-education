namespace AaramEducation.Core.Entities;

public class Quiz
{
    public int QuizId { get; set; }
    public int LessonId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public string? QuizDescription { get; set; }
    public int PassingScore { get; set; }
    public int MaxAttempts { get; set; } = 3;
    public DateTime CreatedAt { get; set; }

    public Lesson Lesson { get; set; } = null!;
    public ICollection<QuizQuestion> Questions { get; set; } = [];
    public ICollection<QuizAttempt> Attempts { get; set; } = [];
}
