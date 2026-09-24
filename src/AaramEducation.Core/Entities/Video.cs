using System;

namespace AaramEducation.Core.Entities
{
    public class Video
    {
        public int VideoId { get; set; }
        public int LessonId { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
        public DateTime UploadedAt { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
    }
}
