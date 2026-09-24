using System;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Quiz
{
    public partial class QuizStartPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            int quizId = int.TryParse(Request.QueryString["id"], out int qid) ? qid : 0;
            if (quizId == 0) { Response.Redirect("~/Courses/Index.aspx"); return; }

            using (var db = new ApplicationDbContext())
            {
                var repo = new QuizRepository(db);
                var quiz = repo.GetWithQuestionsAsync(quizId).Result;
                if (quiz == null) { Response.Redirect("~/Courses/Index.aspx"); return; }

                Page.Title = quiz.QuizTitle;
                litTitle.Text = System.Web.HttpUtility.HtmlEncode(quiz.QuizTitle);
                litMeta.Text = quiz.Questions.Count + " questions &bull; Pass: " + quiz.PassingScore + "%";
                litDesc.Text = System.Web.HttpUtility.HtmlEncode(quiz.Description ?? "Test your knowledge with this quiz.");
                lnkStart.NavigateUrl = "~/Quiz/Take.aspx?id=" + quizId;
            }
        }
    }
}
