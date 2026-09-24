using System;

namespace AaramEducation.Core.Entities
{
    public class QuestionResponse
    {
        public int ResponseId { get; set; }
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int? SelectedOptionId { get; set; }
        public string? TextResponse { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }

        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual QuizQuestion Question { get; set; } = null!;
        public virtual AnswerOption? SelectedOption { get; set; }
    }
}
