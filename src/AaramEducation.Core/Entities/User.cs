using System;
using System.Collections.Generic;
using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserRole Role { get; set; }
        public int TotalXpPoints { get; set; }
        public int CurrentStreakDays { get; set; }
        public int LongestStreakDays { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
        public virtual ICollection<ModuleProgress> ModuleProgresses { get; set; } = new List<ModuleProgress>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
        public virtual ICollection<DailyActivityLog> DailyActivityLogs { get; set; } = new List<DailyActivityLog>();
        public virtual ICollection<GuestbookEntry> ModeratedEntries { get; set; } = new List<GuestbookEntry>();
    }
}
