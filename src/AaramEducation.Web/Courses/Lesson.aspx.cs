using System;
using System.Linq;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Courses
{
    public partial class LessonPage : BasePage
    {
        private int _lessonId;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (!int.TryParse(Request.QueryString["lessonId"], out _lessonId))
            { ShowError("Invalid lesson."); return; }

            if (!IsPostBack) LoadLesson();
        }

        private void LoadLesson()
        {
            using (var db = new ApplicationDbContext())
            {
                var lessonRepo = new LessonRepository(db);
                var lesson = lessonRepo.GetWithContentAsync(_lessonId).Result;
                if (lesson == null) { ShowError("Lesson not found."); return; }

                if (!lesson.IsFreeSample)
                {
                    var enrollRepo = new EnrollmentRepository(db);
                    bool enrolled = enrollRepo.IsEnrolledAsync(CurrentUserId!.Value, lesson.Module.Course.CourseId).Result;
                    if (!enrolled) { ShowError("You are not enrolled in this course."); return; }
                }

                pnlLesson.Visible = true;
                litTitle.Text = System.Web.HttpUtility.HtmlEncode(lesson.LessonTitle);
                litModuleName.Text = System.Web.HttpUtility.HtmlEncode(lesson.Module?.ModuleName ?? "");
                litLessonTitle.Text = System.Web.HttpUtility.HtmlEncode(lesson.LessonTitle);
                aCourse.HRef = "~/Courses/Details.aspx?id=" + lesson.Module?.Course?.CourseId;
                aCourse.InnerText = lesson.Module?.Course?.CourseName ?? "Course";

                var video = lesson.Videos.FirstOrDefault();
                pnlVideo.Visible = video != null;
                if (video != null)
                {
                    videoSrc.Attributes["src"] = ResolveUrl(video.VideoUrl);
                    videoSrc.Attributes["type"] = "video/mp4";
                }

                var note = lesson.StudyNotes.FirstOrDefault();
                pnlNotes.Visible = note != null;
                if (note != null)
                {
                    litNoteContent.Text = note.NoteContent;
                    aNoteFile.Visible = !string.IsNullOrEmpty(note.FileUrl);
                    if (!string.IsNullOrEmpty(note.FileUrl))
                        aNoteFile.HRef = ResolveUrl(note.FileUrl);
                }

                pnlQuiz.Visible = lesson.Quizzes.Any();
                if (lesson.Quizzes.Any())
                {
                    rptQuizzes.DataSource = lesson.Quizzes.ToList();
                    rptQuizzes.DataBind();
                }

                var allLessons = lesson.Module?.Lessons?.OrderBy(l => l.SequenceOrder).ToList();
                var idx = allLessons?.FindIndex(l => l.LessonId == _lessonId) ?? -1;
                if (allLessons != null && idx >= 0 && idx < allLessons.Count - 1)
                {
                    aNext.Visible = true;
                    aNext.HRef = "~/Courses/Lesson.aspx?lessonId=" + allLessons[idx + 1].LessonId;
                }
                else aNext.Visible = false;
            }
        }

        protected void MarkComplete_Click(object sender, EventArgs e)
        {
            RequireAuth();
            using (var db = new ApplicationDbContext())
            {
                var progressRepo = new ProgressRepository(db);
                progressRepo.UpsertLessonProgressAsync(CurrentUserId!.Value, _lessonId, ProgressStatus.Completed).Wait();
                progressRepo.UpdateRollupsAsync(CurrentUserId!.Value, _lessonId).Wait();
                new BadgeRepository(db).CheckAndAwardAsync(CurrentUserId!.Value).Wait();
            }
            Response.Redirect(Request.RawUrl);
        }

        private void ShowError(string msg)
        {
            lblError.Text = System.Web.HttpUtility.HtmlEncode(msg);
            lblError.Visible = true;
            pnlLesson.Visible = false;
        }
    }
}
