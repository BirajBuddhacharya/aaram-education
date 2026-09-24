using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Quizzes;

public class QuizResultViewModel
{
    public QuizAttempt Attempt { get; set; } = null!;
    public bool Passed => Attempt.ScoreAchieved >= Attempt.Quiz.PassingScore;
}
