using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

public class QuizQuestion
{
    public int QuestionId { get; set; }
    public int QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public int SequenceOrder { get; set; }
    public int PointsValue { get; set; } = 1;

    public Quiz Quiz { get; set; } = null!;
    public ICollection<AnswerOption> Options { get; set; } = [];
    public ICollection<QuestionResponse> Responses { get; set; } = [];
}
