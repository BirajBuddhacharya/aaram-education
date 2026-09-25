using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;

namespace AaramEducation.Infrastructure.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly ApplicationDbContext _db;
        public BadgeRepository(ApplicationDbContext db) { _db = db; }

        public async Task<IEnumerable<Badge>> GetAllAsync() =>
            await _db.Badges.AsNoTracking().OrderBy(b => b.TargetValue).ToListAsync().ConfigureAwait(false);

        public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId) =>
            await _db.UserBadges.AsNoTracking()
                .Include(ub => ub.Badge)
                .Where(ub => ub.UserId == userId)
                .OrderByDescending(ub => ub.EarnedAt)
                .ToListAsync().ConfigureAwait(false);

        public async Task CheckAndAwardAsync(int userId)
        {
            var badges = await _db.Badges.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var earnedBadgeIds = await _db.UserBadges
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.BadgeId)
                .ToListAsync().ConfigureAwait(false);

            var user = await _db.Users.FindAsync(userId).ConfigureAwait(false);
            if (user is null) return;

            var now = DateTime.UtcNow;

            foreach (var badge in badges)
            {
                if (earnedBadgeIds.Contains(badge.BadgeId)) continue;

                int currentValue;
                switch (badge.TargetType)
                {
                    case "lessons_completed":
                        currentValue = await _db.LessonProgresses
                            .CountAsync(lp => lp.StudentId == userId && lp.Status == ProgressStatus.Completed).ConfigureAwait(false);
                        break;
                    case "quizzes_passed":
                        currentValue = await _db.QuizAttempts
                            .CountAsync(a => a.StudentId == userId && a.ScoreAchieved >= 60).ConfigureAwait(false);
                        break;
                    case "streak_days":
                        currentValue = user.CurrentStreakDays;
                        break;
                    case "courses_completed":
                        currentValue = await _db.CourseProgresses
                            .CountAsync(cp => cp.Enrollment.StudentId == userId && cp.Status == ProgressStatus.Completed).ConfigureAwait(false);
                        break;
                    default:
                        currentValue = 0;
                        break;
                }

                if (currentValue < badge.TargetValue) continue;

                _db.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.BadgeId,
                    EarnedAt = now,
                });
                user.TotalXpPoints += badge.XpReward;
            }

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
