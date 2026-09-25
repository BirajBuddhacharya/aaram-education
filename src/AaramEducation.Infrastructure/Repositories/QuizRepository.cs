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
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _db;
        public QuizRepository(ApplicationDbContext db) { _db = db; }

        public async Task<Quiz?> GetWithQuestionsAsync(int quizId)
        {
            // EF6 can't sort inside Include — load then sort in memory
            var quiz = await _db.Quizzes.AsNoTracking()
                .Include(q => q.Questions.Select(qq => qq.Options))
                .FirstOrDefaultAsync(q => q.QuizId == quizId).ConfigureAwait(false);

            if (quiz != null)
            {
                quiz.Questions = quiz.Questions.OrderBy(qq => qq.SequenceOrder).ToList();
                foreach (var question in quiz.Questions)
                    question.Options = question.Options.OrderBy(o => o.SequenceOrder).ToList();
            }
            return quiz;
        }

        public Task<int> GetAttemptCountAsync(int studentId, int quizId) =>
            _db.QuizAttempts.CountAsync(a => a.StudentId == studentId && a.QuizId == quizId);

        public async Task<IEnumerable<QuizAttempt>> GetAttemptsByStudentAsync(int studentId, int quizId) =>
            await _db.QuizAttempts.AsNoTracking()
                .Where(a => a.StudentId == studentId && a.QuizId == quizId)
                .OrderByDescending(a => a.AttemptDate)
                .ToListAsync().ConfigureAwait(false);

        public async Task<QuizAttempt?> GetAttemptWithResponsesAsync(int attemptId)
        {
            var attempt = await _db.QuizAttempts.AsNoTracking()
                .Include(a => a.Quiz.Questions.Select(qq => qq.Options))
                .Include(a => a.Responses.Select(r => r.SelectedOption))
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId).ConfigureAwait(false);

            if (attempt?.Quiz != null)
                attempt.Quiz.Questions = attempt.Quiz.Questions.OrderBy(qq => qq.SequenceOrder).ToList();

            return attempt;
        }

        public async Task<QuizAttempt> StartAttemptAsync(int studentId, int quizId)
        {
            var attempt = new QuizAttempt
            {
                StudentId = studentId,
                QuizId = quizId,
                AttemptDate = DateTime.UtcNow,
                AttemptStatus = AttemptStatus.InProgress,
            };
            _db.QuizAttempts.Add(attempt);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return attempt;
        }

        public async Task<QuizAttempt> SubmitAttemptAsync(
            int attemptId,
            IEnumerable<(int questionId, int? optionId, string? text)> responses)
        {
            var attempt = await _db.QuizAttempts
                .Include(a => a.Quiz.Questions.Select(qq => qq.Options))
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Attempt {attemptId} not found.");

            var now = DateTime.UtcNow;
            int score = 0;

            foreach (var (questionId, optionId, text) in responses)
            {
                var question = attempt.Quiz.Questions.FirstOrDefault(q => q.QuestionId == questionId);
                if (question is null) continue;

                bool isCorrect = question.QuestionType == QuestionType.ShortAnswer
                    ? false
                    : optionId.HasValue && question.Options.Any(o => o.OptionId == optionId && o.IsCorrect);

                if (isCorrect) score += question.PointsValue;

                _db.QuestionResponses.Add(new QuestionResponse
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

            await _db.SaveChangesAsync().ConfigureAwait(false);
            return attempt;
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return quiz;
        }

        public async Task UpdateAsync(Quiz quiz)
        {
            _db.Entry(quiz).State = EntityState.Modified;
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int quizId)
        {
            var quiz = await _db.Quizzes.FindAsync(quizId).ConfigureAwait(false);
            if (quiz is not null)
            {
                _db.Quizzes.Remove(quiz);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
