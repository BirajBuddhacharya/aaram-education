using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Quiz
{
    public partial class QuizTakePage : BasePage
    {
        private List<QuizQuestion> _questions = new List<QuizQuestion>();

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            int quizId = int.TryParse(Request.QueryString["id"], out int qid) ? qid : 0;
            if (quizId == 0) { Response.Redirect("~/Courses/Index.aspx"); return; }
            hfQuizId.Value = quizId.ToString();

            using (var db = new ApplicationDbContext())
            {
                var repo = new QuizRepository(db);
                var quiz = repo.GetWithQuestionsAsync(quizId).Result;
                if (quiz == null) { Response.Redirect("~/Quiz/Start.aspx?id=" + quizId); return; }

                _questions = quiz.Questions.OrderBy(q => q.QuestionOrder).ToList();
                litQuizTitle.Text = System.Web.HttpUtility.HtmlEncode(quiz.QuizTitle);
                litQTotal.Text = _questions.Count.ToString();

                if (!IsPostBack)
                {
                    var attempt = new QuizAttempt
                    {
                        StudentId = CurrentUserId!.Value,
                        QuizId = quizId,
                        StartedAt = DateTime.UtcNow,
                        Status = AttemptStatus.InProgress,
                    };
                    db.QuizAttempts.Add(attempt);
                    db.SaveChanges();
                    hfAttemptId.Value = attempt.AttemptId.ToString();
                    hfQuestionIndex.Value = "0";
                    LoadQuestion(0);
                }
            }
        }

        private void LoadQuestion(int index)
        {
            using (var db = new ApplicationDbContext())
            {
                var repo = new QuizRepository(db);
                var quiz = repo.GetWithQuestionsAsync(int.Parse(hfQuizId.Value)).Result;
                var questions = quiz!.Questions.OrderBy(q => q.QuestionOrder).ToList();
                if (index >= questions.Count) return;
                var q = questions[index];
                litQNum.Text = (index + 1).ToString();
                litQuestion.Text = System.Web.HttpUtility.HtmlEncode(q.QuestionText);

                if (q.QuestionType == QuestionType.MultipleChoice || q.QuestionType == QuestionType.TrueFalse)
                {
                    rblOptions.Visible = true;
                    txtShortAnswer.Visible = false;
                    rblOptions.Items.Clear();
                    foreach (var opt in q.Options.OrderBy(o => o.OptionId))
                        rblOptions.Items.Add(new System.Web.UI.WebControls.ListItem(opt.OptionText, opt.OptionId.ToString()));
                }
                else
                {
                    rblOptions.Visible = false;
                    txtShortAnswer.Visible = true;
                }
            }
        }

        protected void Next_Click(object sender, EventArgs e)
        {
            int index = int.Parse(hfQuestionIndex.Value);
            int attemptId = int.Parse(hfAttemptId.Value);

            using (var db = new ApplicationDbContext())
            {
                var repo = new QuizRepository(db);
                var quiz = repo.GetWithQuestionsAsync(int.Parse(hfQuizId.Value)).Result;
                var questions = quiz!.Questions.OrderBy(q => q.QuestionOrder).ToList();

                if (index < questions.Count)
                {
                    var q = questions[index];
                    var response = new QuestionResponse
                    {
                        AttemptId = attemptId,
                        QuestionId = q.QuestionId,
                        AnsweredAt = DateTime.UtcNow,
                    };

                    if (q.QuestionType == QuestionType.MultipleChoice || q.QuestionType == QuestionType.TrueFalse)
                    {
                        if (rblOptions.SelectedValue != "")
                        {
                            int optId = int.Parse(rblOptions.SelectedValue);
                            response.SelectedOptionId = optId;
                            response.IsCorrect = q.Options.FirstOrDefault(o => o.OptionId == optId)?.IsCorrect ?? false;
                        }
                    }
                    else
                    {
                        response.TextAnswer = txtShortAnswer.Text.Trim();
                        response.IsCorrect = false;
                    }

                    db.QuestionResponses.Add(response);
                    db.SaveChanges();
                }

                index++;
                hfQuestionIndex.Value = index.ToString();

                if (index >= questions.Count)
                {
                    var attempt = db.QuizAttempts.Find(attemptId);
                    if (attempt != null)
                    {
                        var responses = db.QuestionResponses.Where(r => r.AttemptId == attemptId).ToList();
                        int correct = responses.Count(r => r.IsCorrect);
                        int total = questions.Count;
                        attempt.CompletedAt = DateTime.UtcNow;
                        attempt.Score = total == 0 ? 0 : (int)Math.Round((double)correct / total * 100);
                        attempt.Status = attempt.Score >= quiz.PassingScore ? AttemptStatus.Passed : AttemptStatus.Failed;
                        db.Entry(attempt).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    Response.Redirect("~/Quiz/Results.aspx?attemptId=" + attemptId);
                    return;
                }

                LoadQuestion(index);
            }
        }
    }
}
