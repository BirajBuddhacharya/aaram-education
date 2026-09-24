using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities;

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

    public ICollection<Enrollment> Enrollments { get; set; } = [];
    public ICollection<LessonProgress> LessonProgresses { get; set; } = [];
    public ICollection<ModuleProgress> ModuleProgresses { get; set; } = [];
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
    public ICollection<UserBadge> UserBadges { get; set; } = [];
    public ICollection<DailyActivityLog> DailyActivityLogs { get; set; } = [];
    public ICollection<GuestbookEntry> ModeratedEntries { get; set; } = [];
}
