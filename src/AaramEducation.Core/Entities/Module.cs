using System;
using System.Collections.Generic;

namespace AaramEducation.Core.Entities
{
    public class Module
    {
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string? ModuleDescription { get; set; }
        public int SequenceOrder { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual ICollection<ModuleProgress> ModuleProgresses { get; set; } = new List<ModuleProgress>();
    }
}
