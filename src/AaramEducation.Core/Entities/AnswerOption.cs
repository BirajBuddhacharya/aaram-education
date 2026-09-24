using System.Collections.Generic;

namespace AaramEducation.Core.Entities
{
    public class AnswerOption
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int SequenceOrder { get; set; }

        public virtual QuizQuestion Question { get; set; } = null!;
        public virtual ICollection<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();
    }
}
