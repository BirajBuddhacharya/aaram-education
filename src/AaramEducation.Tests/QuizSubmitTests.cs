using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AaramEducation.Tests;

public class QuizSubmitTests
{
    private static ApplicationDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    private static async Task<ApplicationDbContext> SeedAsync()
    {
        var db = CreateDb();
        db.Users.AddRange(
            new User { UserId = 99, Email = "t@test.com", FirstName = "T", LastName = "T", PasswordHash = "x", Role = UserRole.Tutor, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { UserId = 1,  Email = "s@test.com", FirstName = "S", LastName = "S", PasswordHash = "x", Role = UserRole.Student, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );
        db.Lessons.Add(new Lesson { LessonId = 1, ModuleId = 1, LessonTitle = "L1", SequenceOrder = 1, IsFreeSample = false, CreatedAt = DateTime.UtcNow });
        db.Quizzes.Add(new Quiz { QuizId = 1, LessonId = 1, QuizTitle = "Q1", PassingScore = 60, MaxAttempts = 3, CreatedAt = DateTime.UtcNow });
        db.QuizQuestions.Add(new QuizQuestion { QuestionId = 1, QuizId = 1, QuestionText = "2+2?", QuestionType = QuestionType.MultipleChoice, SequenceOrder = 1, PointsValue = 1 });
        db.AnswerOptions.AddRange(
            new AnswerOption { OptionId = 1, QuestionId = 1, OptionText = "4", IsCorrect = true,  SequenceOrder = 1 },
            new AnswerOption { OptionId = 2, QuestionId = 1, OptionText = "5", IsCorrect = false, SequenceOrder = 2 }
        );
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task Submit_Scores100_WhenCorrectOptionSelected()
    {
        await using var db = await SeedAsync();
        db.QuizAttempts.Add(new QuizAttempt { AttemptId = 1, StudentId = 1, QuizId = 1, AttemptDate = DateTime.UtcNow, AttemptStatus = AttemptStatus.InProgress });
        await db.SaveChangesAsync();

        var graded = await new QuizRepository(db).SubmitAttemptAsync(1, [(1, 1, null)]);
        Assert.Equal(100, graded.ScoreAchieved);
    }

    [Fact]
    public async Task Submit_Scores0_WhenWrongOptionSelected()
    {
        await using var db = await SeedAsync();
        db.QuizAttempts.Add(new QuizAttempt { AttemptId = 1, StudentId = 1, QuizId = 1, AttemptDate = DateTime.UtcNow, AttemptStatus = AttemptStatus.InProgress });
        await db.SaveChangesAsync();

        var graded = await new QuizRepository(db).SubmitAttemptAsync(1, [(1, 2, null)]);
        Assert.Equal(0, graded.ScoreAchieved);
    }
}
