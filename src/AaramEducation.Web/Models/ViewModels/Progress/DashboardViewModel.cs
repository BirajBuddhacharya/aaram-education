using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;

namespace AaramEducation.Web.Models.ViewModels.Progress;

public class CourseProgressCard
{
    public Enrollment Enrollment { get; set; } = null!;
    public CourseProgress? Progress { get; set; }
    public float PercentComplete => Progress?.PercentComplete ?? 0f;
    public ProgressStatus Status => Progress?.Status ?? ProgressStatus.NotStarted;
}

public class DashboardViewModel
{
    public AaramEducation.Core.Entities.User Student { get; set; } = null!;
    public IEnumerable<CourseProgressCard> EnrolledCourses { get; set; } = [];
    public IEnumerable<UserBadge> RecentBadges { get; set; } = [];
    public int BadgeCount { get; set; }
    public IEnumerable<DailyActivityLog> RecentActivity { get; set; } = [];
}
