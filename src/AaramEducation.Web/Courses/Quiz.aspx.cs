using System;
using System.Linq;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Courses
{
    public partial class QuizPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (!IsPostBack)
            {
                if (!int.TryParse(Request.QueryString["quizId"], out int quizId))
                { ShowError("Invalid quiz."); return; }

                using (var db = new ApplicationDbContext())
                {
                    var repo = new QuizRepository(db);
                    var quiz = repo.GetWithQuestionsAsync(quizId).Result;
                    if (quiz == null) { ShowError("Quiz not found."); return; }

                    int attempts = repo.GetAttemptCountAsync(CurrentUserId!.Value, quizId).Result;
                    if (quiz.MaxAttempts > 0 && attempts >= quiz.MaxAttempts)
                    { ShowError($"Maximum attempts ({quiz.MaxAttempts}) reached."); return; }

                    var attempt = repo.StartAttemptAsync(CurrentUserId!.Value, quizId).Result;
                    hfQuizId.Value = quizId.ToString();
                    hfAttemptId.Value = attempt.AttemptId.ToString();
                    litTitle.Text = System.Web.HttpUtility.HtmlEncode(quiz.QuizTitle);
                    litPassing.Text = quiz.PassingScore.ToString();

                    rptQuestions.DataSource = quiz.Questions.ToList();
                    rptQuestions.DataBind();
                    pnlQuiz.Visible = true;
                }
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            RequireAuth();
            if (!int.TryParse(hfAttemptId.Value, out int attemptId) ||
                !int.TryParse(hfQuizId.Value, out int quizId))
            { ShowError("Session error. Please try again."); return; }

            using (var db = new ApplicationDbContext())
            {
                var quizRepo = new QuizRepository(db);
                var quiz = quizRepo.GetWithQuestionsAsync(quizId).Result;
                if (quiz == null) { ShowError("Quiz not found."); return; }

                var responses = quiz.Questions
                    .Select(q =>
                    {
                        int? optId = null;
                        string key = "q_" + q.QuestionId;
                        if (int.TryParse(Request.Form[key], out int oid)) optId = oid;
                        return (q.QuestionId, optId, (string?)null);
                    })
                    .ToList();

                var attempt = quizRepo.SubmitAttemptAsync(attemptId, responses).Result;

                pnlQuiz.Visible = false;
                pnlResult.Visible = true;
                litScore.Text = attempt.ScoreAchieved.ToString();
                bool passed = attempt.ScoreAchieved >= quiz.PassingScore;
                litResultMsg.Text = passed
                    ? "<span class=\"text-success fw-semibold\">Passed!</span>"
                    : "<span class=\"text-danger\">Not passed. Review the material and try again.</span>";

                aBack.HRef = quiz.LessonId.HasValue
                    ? "~/Courses/Lesson.aspx?lessonId=" + quiz.LessonId.Value
                    : "~/Courses/Index.aspx";

                new BadgeRepository(db).CheckAndAwardAsync(CurrentUserId!.Value).Wait();
            }
        }

        private void ShowError(string msg)
        {
            lblError.Text = System.Web.HttpUtility.HtmlEncode(msg);
            lblError.Visible = true;
            pnlQuiz.Visible = false;
        }
    }
}
