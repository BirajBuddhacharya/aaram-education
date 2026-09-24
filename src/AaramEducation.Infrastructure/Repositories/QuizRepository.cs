using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class QuizRepository(ApplicationDbContext db) : IQuizRepository
{
    public Task<Quiz?> GetWithQuestionsAsync(int quizId) =>
        db.Quizzes.AsNoTracking()
            .Include(q => q.Questions.OrderBy(qq => qq.SequenceOrder))
                .ThenInclude(qq => qq.Options.OrderBy(o => o.SequenceOrder))
            .FirstOrDefaultAsync(q => q.QuizId == quizId);

    public Task<int> GetAttemptCountAsync(int studentId, int quizId) =>
        db.QuizAttempts.CountAsync(a => a.StudentId == studentId && a.QuizId == quizId);

    public async Task<IEnumerable<QuizAttempt>> GetAttemptsByStudentAsync(int studentId, int quizId) =>
        await db.QuizAttempts.AsNoTracking()
            .Where(a => a.StudentId == studentId && a.QuizId == quizId)
            .OrderByDescending(a => a.AttemptDate)
            .ToListAsync();

    public Task<QuizAttempt?> GetAttemptWithResponsesAsync(int attemptId) =>
        db.QuizAttempts.AsNoTracking()
            .Include(a => a.Quiz).ThenInclude(q => q.Questions.OrderBy(qq => qq.SequenceOrder))
                .ThenInclude(qq => qq.Options)
            .Include(a => a.Responses)
                .ThenInclude(r => r.SelectedOption)
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId);

    public async Task<QuizAttempt> StartAttemptAsync(int studentId, int quizId)
    {
        var attempt = new QuizAttempt
        {
            StudentId = studentId,
            QuizId = quizId,
            AttemptDate = DateTime.UtcNow,
            AttemptStatus = AttemptStatus.InProgress,
        };
        db.QuizAttempts.Add(attempt);
        await db.SaveChangesAsync();
        return attempt;
    }

    public async Task<QuizAttempt> SubmitAttemptAsync(
        int attemptId,
        IEnumerable<(int questionId, int? optionId, string? text)> responses)
    {
        var attempt = await db.QuizAttempts
            .Include(a => a.Quiz).ThenInclude(q => q.Questions).ThenInclude(qq => qq.Options)
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId)
            ?? throw new InvalidOperationException($"Attempt {attemptId} not found.");

        var now = DateTime.UtcNow;
        int score = 0;
        int total = attempt.Quiz.Questions.Count;

        foreach (var (questionId, optionId, text) in responses)
        {
            var question = attempt.Quiz.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question is null) continue;

            bool isCorrect = question.QuestionType == QuestionType.ShortAnswer
                ? false  // short answer: manual grading not implemented yet
                : optionId.HasValue && question.Options.Any(o => o.OptionId == optionId && o.IsCorrect);

            if (isCorrect) score += question.PointsValue;

            db.QuestionResponses.Add(new QuestionResponse
            {
                AttemptId = attemptId,
                QuestionId = questionId,
                SelectedOptionId = optionId,
                TextResponse = text,
                IsCorrect = isCorrect,
                AnsweredAt = now,
            });
        }

        int maxPoints = attempt.Quiz.Questions.Sum(q => q.PointsValue);
        attempt.ScoreAchieved = maxPoints > 0 ? (int)Math.Round((double)score / maxPoints * 100) : 0;
        attempt.TimeTakenSeconds = (int)(now - attempt.AttemptDate).TotalSeconds;
        attempt.AttemptStatus = AttemptStatus.Graded;

        await db.SaveChangesAsync();
        return attempt;
    }

    public async Task<Quiz> CreateAsync(Quiz quiz)
    {
        db.Quizzes.Add(quiz);
        await db.SaveChangesAsync();
        return quiz;
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        db.Quizzes.Update(quiz);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int quizId)
    {
        var quiz = await db.Quizzes.FindAsync(quizId);
        if (quiz is not null)
        {
            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();
        }
    }
}
