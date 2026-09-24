using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AaramEducation.Tests;

public class BadgeRepositoryTests
{
    private static ApplicationDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    private static User MakeStudent() => new()
    {
        UserId = 1, Email = "s@test.com", FirstName = "A", LastName = "B",
        PasswordHash = "x", Role = UserRole.Student,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
    };

    private static Badge MakeFirstStepBadge() => new()
    {
        BadgeId = 1, BadgeName = "First Step", TargetType = "lessons_completed",
        TargetValue = 1, XpReward = 50, Description = "Complete 1 lesson", IconUrl = "",
    };

    [Fact]
    public async Task CheckAndAward_GrantsBadge_WhenThresholdMet()
    {
        await using var db = CreateDb();
        db.Users.Add(MakeStudent());
        db.Badges.Add(MakeFirstStepBadge());
        db.LessonProgresses.Add(new LessonProgress
        {
            StudentId = 1, LessonId = 1, Status = ProgressStatus.Completed,
            StartedAt = DateTime.UtcNow, LastAccessedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();

        await new BadgeRepository(db).CheckAndAwardAsync(1);

        var awarded = await db.UserBadges.FirstOrDefaultAsync(ub => ub.UserId == 1 && ub.BadgeId == 1);
        Assert.NotNull(awarded);
        Assert.Equal(50, (await db.Users.FindAsync(1))!.TotalXpPoints);
    }

    [Fact]
    public async Task CheckAndAward_DoesNotDoubleBadge_WhenCalledTwice()
    {
        await using var db = CreateDb();
        db.Users.Add(MakeStudent());
        db.Badges.Add(MakeFirstStepBadge());
        db.LessonProgresses.Add(new LessonProgress
        {
            StudentId = 1, LessonId = 1, Status = ProgressStatus.Completed,
            StartedAt = DateTime.UtcNow, LastAccessedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();

        var repo = new BadgeRepository(db);
        await repo.CheckAndAwardAsync(1);
        await repo.CheckAndAwardAsync(1);

        Assert.Equal(1, await db.UserBadges.CountAsync(ub => ub.UserId == 1 && ub.BadgeId == 1));
    }

    [Fact]
    public async Task CheckAndAward_DoesNotGrantBadge_WhenThresholdNotMet()
    {
        await using var db = CreateDb();
        db.Users.Add(MakeStudent());
        db.Badges.Add(new Badge
        {
            BadgeId = 1, BadgeName = "On a Roll", TargetType = "lessons_completed",
            TargetValue = 10, XpReward = 100, Description = "Complete 10 lessons", IconUrl = "",
        });
        await db.SaveChangesAsync();

        await new BadgeRepository(db).CheckAndAwardAsync(1);

        Assert.Equal(0, await db.UserBadges.CountAsync(ub => ub.UserId == 1));
    }
}
