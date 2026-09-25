using System;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Quiz
{
    public partial class QuizStartPage : BasePage
    {
        private int QuizId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (QuizId == 0) { Response.Redirect("~/Courses/Index.aspx"); return; }
            if (IsPostBack) return;

            using (var db = new ApplicationDbContext())
            {
                var repo = new QuizRepository(db);
                var quiz = repo.GetWithQuestionsAsync(QuizId).Result;
                if (quiz == null) { Response.Redirect("~/Courses/Index.aspx"); return; }

                Page.Title = quiz.QuizTitle;
                litTitle.Text = System.Web.HttpUtility.HtmlEncode(quiz.QuizTitle);
                litDescription.Text = System.Web.HttpUtility.HtmlEncode(
                    quiz.QuizDescription ?? "Test your knowledge with this quiz.");
                litPassing.Text = quiz.PassingScore.ToString();
                litMaxAttempts.Text = quiz.MaxAttempts.ToString();
                lnkBack.NavigateUrl = "~/Lessons/Show.aspx?id=" + quiz.LessonId;

                int used = repo.GetAttemptCountAsync(CurrentUserId!.Value, QuizId).Result;
                litAttemptsUsed.Text = used.ToString();

                btnStart.Enabled = used < quiz.MaxAttempts;
                if (!btnStart.Enabled)
                    litMessage.Text =
                        "<div class=\"alert alert-warning\">You have used all "
                        + quiz.MaxAttempts + " attempts for this quiz.</div>";
            }
        }

        protected void btnStart_Click(object sender, EventArgs e) =>
            Response.Redirect("~/Quiz/Take.aspx?id=" + QuizId);
    }
}
