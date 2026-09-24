using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Quizzes;

public class QuizStartViewModel
{
    public AaramEducation.Core.Entities.Quiz Quiz { get; set; } = null!;
    public int AttemptsUsed { get; set; }
    public IEnumerable<QuizAttempt> PastAttempts { get; set; } = [];
    public bool CanAttempt => AttemptsUsed < Quiz.MaxAttempts;
}
