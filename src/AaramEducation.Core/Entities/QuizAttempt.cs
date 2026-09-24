using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

public class QuizAttempt
{
    public int AttemptId { get; set; }
    public int StudentId { get; set; }
    public int QuizId { get; set; }
    public DateTime AttemptDate { get; set; }
    public int ScoreAchieved { get; set; }
    public int TimeTakenSeconds { get; set; }
    public AttemptStatus AttemptStatus { get; set; }

    public User Student { get; set; } = null!;
    public Quiz Quiz { get; set; } = null!;
    public ICollection<QuestionResponse> Responses { get; set; } = [];
}
