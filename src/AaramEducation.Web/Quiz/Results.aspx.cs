using System;
using System.Data.Entity;
using System.Linq;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Quiz
{
    public partial class QuizResultsPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            int attemptId = int.TryParse(Request.QueryString["attemptId"], out int aid) ? aid : 0;
            if (attemptId == 0) { Response.Redirect("~/Courses/Index.aspx"); return; }

            using (var db = new ApplicationDbContext())
            {
                var attempt = db.QuizAttempts
                    .Include(a => a.Quiz)
                    .Include(a => a.Responses.Select(r => r.Question))
                    .Include(a => a.Responses.Select(r => r.SelectedOption))
                    .FirstOrDefault(a => a.AttemptId == attemptId && a.StudentId == CurrentUserId!.Value);

                if (attempt == null) { Response.Redirect("~/Courses/Index.aspx"); return; }

                litScore.Text = attempt.Score.ToString();
                bool passed = attempt.Status == Core.Enums.AttemptStatus.Passed;
                lblStatus.Text = passed ? "Passed" : "Failed";
                lblStatus.CssClass = "badge fs-6 bg-" + (passed ? "success" : "danger");

                int correct = attempt.Responses.Count(r => r.IsCorrect);
                litDetails.Text = correct + " / " + attempt.Responses.Count + " correct &bull; Pass mark: " + attempt.Quiz.PassingScore + "%";

                lnkRetry.NavigateUrl = "~/Quiz/Start.aspx?id=" + attempt.QuizId;

                var quizLesson = db.Lessons.FirstOrDefault(l => l.Quizzes.Any(q => q.QuizId == attempt.QuizId));
                lnkLesson.NavigateUrl = quizLesson != null ? "~/Lessons/Show.aspx?id=" + quizLesson.LessonId : "~/Courses/Index.aspx";

                rptResponses.DataSource = attempt.Responses.OrderBy(r => r.Question.QuestionOrder).ToList();
                rptResponses.DataBind();
            }
        }
    }
}
