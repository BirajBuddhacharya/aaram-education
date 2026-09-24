using System;
using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }

        public virtual User Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
        public virtual CourseProgress? CourseProgress { get; set; }
    }
}
