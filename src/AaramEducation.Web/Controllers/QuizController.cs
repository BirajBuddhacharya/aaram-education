using System.Security.Claims;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Web.Models.ViewModels.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

[Authorize]
public class QuizController(
    IQuizRepository quizzes,
    IProgressRepository progress,
    IBadgeRepository badges) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Start(int quizId)
    {
        var quiz = await quizzes.GetWithQuestionsAsync(quizId);
        if (quiz is null) return NotFound();

        var studentId = GetStudentId();
        if (studentId is null) return RedirectToAction("Login", "Account");

        var used = await quizzes.GetAttemptCountAsync(studentId.Value, quizId);
        var past = await quizzes.GetAttemptsByStudentAsync(studentId.Value, quizId);

        return View(new QuizStartViewModel
        {
            Quiz = quiz,
            AttemptsUsed = used,
            PastAttempts = past,
        });
    }

    [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(int quizId, string? _)
    {
        var quiz = await quizzes.GetWithQuestionsAsync(quizId);
        if (quiz is null) return NotFound();

        var studentId = GetStudentId();
        if (studentId is null) return RedirectToAction("Login", "Account");

        var used = await quizzes.GetAttemptCountAsync(studentId.Value, quizId);
        if (used >= quiz.MaxAttempts)
            return RedirectToAction("Start", new { quizId });

        var attempt = await quizzes.StartAttemptAsync(studentId.Value, quizId);
        await progress.UpsertDailyLogAsync(studentId.Value, log => log.QuizzesAttempted++);

        return RedirectToAction("Take", new { attemptId = attempt.AttemptId });
    }

    [HttpGet]
    public async Task<IActionResult> Take(int attemptId)
    {
        var attempt = await quizzes.GetAttemptWithResponsesAsync(attemptId);
        if (attempt is null) return NotFound();

        var studentId = GetStudentId();
        if (attempt.StudentId != studentId) return Forbid();

        if (attempt.AttemptStatus != AttemptStatus.InProgress)
            return RedirectToAction("Results", new { attemptId });

        return View(new QuizTakeViewModel
        {
            Quiz = attempt.Quiz,
            AttemptId = attemptId,
            StartedAt = attempt.AttemptDate,
        });
    }

    [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(int attemptId, IFormCollection form)
    {
        var attempt = await quizzes.GetAttemptWithResponsesAsync(attemptId);
        if (attempt is null) return NotFound();

        var studentId = GetStudentId();
        if (attempt.StudentId != studentId) return Forbid();

        var responses = attempt.Quiz.Questions.Select(q =>
        {
            var raw = form[$"q_{q.QuestionId}"].FirstOrDefault();
            int? optionId = int.TryParse(raw, out var oid) ? oid : null;
            string? text = optionId is null ? raw : null;
            return (q.QuestionId, optionId, text);
        });

        var graded = await quizzes.SubmitAttemptAsync(attemptId, responses);

        if (graded.ScoreAchieved >= attempt.Quiz.PassingScore)
            await progress.UpsertDailyLogAsync(studentId!.Value, log => log.QuizzesPassed++);

        await badges.CheckAndAwardAsync(studentId!.Value);

        return RedirectToAction("Results", new { attemptId });
    }

    [HttpGet]
    public async Task<IActionResult> Results(int attemptId)
    {
        var attempt = await quizzes.GetAttemptWithResponsesAsync(attemptId);
        if (attempt is null) return NotFound();

        var studentId = GetStudentId();
        if (attempt.StudentId != studentId) return Forbid();

        return View(new QuizResultViewModel { Attempt = attempt });
    }

    private int? GetStudentId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
