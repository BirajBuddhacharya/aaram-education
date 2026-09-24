using System;
using System.Collections.Generic;

namespace AaramEducation.Core.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public int CreatedByUserId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseDescription { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
