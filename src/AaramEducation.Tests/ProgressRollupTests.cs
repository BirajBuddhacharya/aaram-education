using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Module = AaramEducation.Core.Entities.Module;

namespace AaramEducation.Tests;

// EF InMemory provider's cycle detection rejects the Include chain in UpdateRollupsAsync.
// SQLite in-memory is used instead — it runs real SQL and handles cycles fine.
public class ProgressRollupTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _db;

    public ProgressRollupTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new ApplicationDbContext(opts);
        _db.Database.EnsureCreated();
        Seed().GetAwaiter().GetResult();
    }

    private async Task Seed()
    {
        _db.Users.AddRange(
            new User { UserId = 99, Email = "t@test.com", FirstName = "T", LastName = "T", PasswordHash = "x", Role = UserRole.Tutor, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { UserId = 1,  Email = "s@test.com", FirstName = "S", LastName = "S", PasswordHash = "x", Role = UserRole.Student, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );
        _db.Courses.Add(new Course { CourseId = 1, CreatedByUserId = 99, CourseName = "Test", CourseDescription = "x", Subject = "Math", DifficultyLevel = "Beginner", IsPublished = true, CreatedAt = DateTime.UtcNow });
        _db.Modules.Add(new Module { ModuleId = 1, CourseId = 1, ModuleName = "M1", SequenceOrder = 1, CreatedAt = DateTime.UtcNow });
        _db.Lessons.Add(new Lesson { LessonId = 1, ModuleId = 1, LessonTitle = "L1", SequenceOrder = 1, IsFreeSample = false, CreatedAt = DateTime.UtcNow });
        _db.Enrollments.Add(new Enrollment { EnrollmentId = 1, StudentId = 1, CourseId = 1, EnrollmentDate = DateTime.UtcNow, EnrollmentStatus = EnrollmentStatus.Active });
        _db.CourseProgresses.Add(new CourseProgress { CourseProgressId = 1, EnrollmentId = 1, Status = ProgressStatus.NotStarted, PercentComplete = 0f, LastAccessedAt = DateTime.UtcNow });
        _db.LessonProgresses.Add(new LessonProgress { StudentId = 1, LessonId = 1, Status = ProgressStatus.Completed, StartedAt = DateTime.UtcNow, LastAccessedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateRollupsAsync_SetsModuleCompleted_WhenAllLessonsComplete()
    {
        await new ProgressRepository(_db).UpdateRollupsAsync(1, 1);

        var mp = await _db.ModuleProgresses.FirstOrDefaultAsync(m => m.StudentId == 1 && m.ModuleId == 1);
        Assert.NotNull(mp);
        Assert.Equal(ProgressStatus.Completed, mp!.Status);
    }

    [Fact]
    public async Task UpdateRollupsAsync_SetsCourseCompleted_WhenAllLessonsComplete()
    {
        await new ProgressRepository(_db).UpdateRollupsAsync(1, 1);

        var cp = await _db.CourseProgresses.FirstOrDefaultAsync(c => c.EnrollmentId == 1);
        Assert.NotNull(cp);
        Assert.Equal(ProgressStatus.Completed, cp!.Status);
        Assert.Equal(100f, cp.PercentComplete, precision: 1);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}
