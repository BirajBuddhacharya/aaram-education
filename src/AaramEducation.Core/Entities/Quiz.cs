using System;
using System.Collections.Generic;

namespace AaramEducation.Core.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int LessonId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public string? QuizDescription { get; set; }
        public int PassingScore { get; set; }
        public int MaxAttempts { get; set; } = 3;
        public DateTime CreatedAt { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
