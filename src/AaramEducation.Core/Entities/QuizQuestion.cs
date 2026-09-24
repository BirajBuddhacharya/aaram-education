using System.Collections.Generic;
using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities
{
    public class QuizQuestion
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public int SequenceOrder { get; set; }
        public int PointsValue { get; set; } = 1;

        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
        public virtual ICollection<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();
    }
}
