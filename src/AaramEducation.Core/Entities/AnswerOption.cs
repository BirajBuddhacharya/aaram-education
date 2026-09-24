namespace AaramEducation.Core.Entities;

public class AnswerOption
{
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int SequenceOrder { get; set; }

    public QuizQuestion Question { get; set; } = null!;
    public ICollection<QuestionResponse> Responses { get; set; } = [];
}
