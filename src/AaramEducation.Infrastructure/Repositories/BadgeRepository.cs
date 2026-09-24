using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class BadgeRepository(ApplicationDbContext db) : IBadgeRepository
{
    public async Task<IEnumerable<Badge>> GetAllAsync() =>
        await db.Badges.AsNoTracking().OrderBy(b => b.TargetValue).ToListAsync();

    public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId) =>
        await db.UserBadges.AsNoTracking()
            .Include(ub => ub.Badge)
            .Where(ub => ub.UserId == userId)
            .OrderByDescending(ub => ub.EarnedAt)
            .ToListAsync();

    public async Task CheckAndAwardAsync(int userId)
    {
        var badges = await db.Badges.AsNoTracking().ToListAsync();
        var earnedBadgeIds = await db.UserBadges
            .Where(ub => ub.UserId == userId)
            .Select(ub => ub.BadgeId)
            .ToListAsync();

        var user = await db.Users.FindAsync(userId);
        if (user is null) return;

        var now = DateTime.UtcNow;

        foreach (var badge in badges)
        {
            if (earnedBadgeIds.Contains(badge.BadgeId)) continue;

            int currentValue = badge.TargetType switch
            {
                "lessons_completed" => await db.LessonProgresses
                    .CountAsync(lp => lp.StudentId == userId &&
                                      lp.Status == Core.Enums.ProgressStatus.Completed),
                "quizzes_passed" => await db.QuizAttempts
                    .CountAsync(a => a.StudentId == userId && a.ScoreAchieved >= 60),
                "streak_days"       => user.CurrentStreakDays,
                "courses_completed" => await db.CourseProgresses
                    .CountAsync(cp => cp.Enrollment.StudentId == userId &&
                                      cp.Status == Core.Enums.ProgressStatus.Completed),
                _ => 0
            };

            if (currentValue < badge.TargetValue) continue;

            db.UserBadges.Add(new UserBadge
            {
                UserId = userId,
                BadgeId = badge.BadgeId,
                EarnedAt = now,
            });
            user.TotalXpPoints += badge.XpReward;
        }

        await db.SaveChangesAsync();
    }
}
