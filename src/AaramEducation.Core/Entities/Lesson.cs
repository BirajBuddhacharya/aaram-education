using System;
using System.Collections.Generic;

namespace AaramEducation.Core.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int ModuleId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string? LessonDescription { get; set; }
        public int SequenceOrder { get; set; }
        public bool IsFreeSample { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Module Module { get; set; } = null!;
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
        public virtual ICollection<StudyNote> StudyNotes { get; set; } = new List<StudyNote>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    }
}
