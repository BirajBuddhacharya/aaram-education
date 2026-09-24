namespace AaramEducation.Core.Entities;

public class DailyActivityLog
{
    public int LogId { get; set; }
    public int UserId { get; set; }
    public DateOnly ActivityDate { get; set; }
    public int LoginCount { get; set; }
    public int LessonsStarted { get; set; }
    public int LessonsCompleted { get; set; }
    public int QuizzesAttempted { get; set; }
    public int QuizzesPassed { get; set; }
    public int NotesDownloaded { get; set; }
    public int VideoWatchSeconds { get; set; }
    public int TotalTimeSeconds { get; set; }
    public int XpEarned { get; set; }

    public User User { get; set; } = null!;
}
