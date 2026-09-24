namespace AaramEducation.Web.Models.ViewModels.Quizzes;

public class QuizTakeViewModel
{
    public AaramEducation.Core.Entities.Quiz Quiz { get; set; } = null!;
    public int AttemptId { get; set; }
    public DateTime StartedAt { get; set; }
}
