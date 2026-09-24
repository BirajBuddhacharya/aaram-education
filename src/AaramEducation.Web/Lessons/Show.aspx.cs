using System;
using System.Linq;
using System.Web;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Lessons
{
    public partial class Show : BasePage
    {
        private int LessonId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (!IsPostBack) LoadLesson();
        }

        private void LoadLesson()
        {
            using (var db = new ApplicationDbContext())
            {
                var lessonRepo = new LessonRepository(db);
                var progressRepo = new ProgressRepository(db);

                var lesson = lessonRepo.GetWithContentAsync(LessonId).Result;
                if (lesson == null) { Response.Redirect("~/Courses/Index.aspx"); return; }

                var module = lesson.Module;
                var course = module.Course;

                // Enrollment check (free samples bypass)
                if (!lesson.IsFreeSample)
                {
                    var enrollmentRepo = new EnrollmentRepository(db);
                    bool enrolled = enrollmentRepo.IsEnrolledAsync(CurrentUserId!.Value, course.CourseId).Result;
                    if (!enrolled) { Response.Redirect("~/Courses/Details.aspx?id=" + course.CourseId); return; }
                }

                litTitle.Text = HttpUtility.HtmlEncode(lesson.LessonTitle);
                litBreadcrumb.Text = $"<nav aria-label='breadcrumb'><ol class='breadcrumb'>" +
                    $"<li class='breadcrumb-item'><a href='~/Courses/Index.aspx' runat='server'>Courses</a></li>" +
                    $"<li class='breadcrumb-item'><a href='~/Courses/Details.aspx?id={course.CourseId}'>{HttpUtility.HtmlEncode(course.CourseName)}</a></li>" +
                    $"<li class='breadcrumb-item active'>{HttpUtility.HtmlEncode(lesson.LessonTitle)}</li></ol></nav>";

                lnkBack.NavigateUrl = ResolveUrl("~/Courses/Details.aspx?id=" + course.CourseId);

                // Video
                var video = lessonRepo.GetVideoAsync(LessonId).Result;
                if (video != null)
                {
                    litVideoTitle.Text = HttpUtility.HtmlEncode(video.VideoTitle);
                    litVideo.Text = $"<iframe src='{HttpUtility.HtmlAttributeEncode(video.VideoUrl)}' allowfullscreen class='w-100 h-100'></iframe>";
                }
                else pnlVideo.Visible = false;

                // Study note
                if (lesson.StudyNotes.Count > 0)
                {
                    var note = lesson.StudyNotes.First();
                    litNoteTitle.Text = HttpUtility.HtmlEncode(note.NoteTitle);
                    litNoteContent.Text = HttpUtility.HtmlEncode(note.NoteContent ?? "");
                }
                else pnlNote.Visible = false;

                // Quizzes
                if (lesson.Quizzes.Count > 0)
                {
                    rptQuizzes.DataSource = lesson.Quizzes.OrderBy(q => q.QuizTitle).ToList();
                    rptQuizzes.DataBind();
                }
                else pnlQuizzes.Visible = false;

                // Progress (authenticated students only)
                if (IsAuthenticated && CurrentUserRole == "Student")
                {
                    var lp = progressRepo.GetLessonProgressAsync(CurrentUserId!.Value, LessonId).Result;
                    if (lp?.Status == ProgressStatus.Completed)
                    {
                        btnComplete.Enabled = false;
                        btnComplete.Text = "Completed";
                        litProgressStatus.Text = "<span class='badge bg-success'>Completed</span>";
                    }
                    else if (lp?.Status == ProgressStatus.InProgress)
                        litProgressStatus.Text = "<span class='badge bg-info'>In Progress</span>";
                }
                else pnlProgress.Visible = false;

                // Prev/next lessons in module
                var siblings = module.Lessons.OrderBy(l => l.SequenceOrder).ToList();
                int idx = siblings.FindIndex(l => l.LessonId == LessonId);
                if (idx > 0) { lnkPrev.NavigateUrl = ResolveUrl("~/Lessons/Show.aspx?id=" + siblings[idx - 1].LessonId); lnkPrev.Visible = true; }
                if (idx < siblings.Count - 1) { lnkNext.NavigateUrl = ResolveUrl("~/Lessons/Show.aspx?id=" + siblings[idx + 1].LessonId); lnkNext.Visible = true; }

                // Mark as in-progress on first view
                if (IsAuthenticated && CurrentUserRole == "Student")
                    progressRepo.UpsertLessonProgressAsync(CurrentUserId!.Value, LessonId, ProgressStatus.InProgress).Wait();
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            using (var db = new ApplicationDbContext())
            {
                var progressRepo = new ProgressRepository(db);
                progressRepo.UpsertLessonProgressAsync(CurrentUserId!.Value, LessonId, ProgressStatus.Completed).Wait();
                progressRepo.UpdateRollupsAsync(CurrentUserId!.Value, LessonId).Wait();
                var badgeRepo = new BadgeRepository(db);
                badgeRepo.CheckAndAwardAsync(CurrentUserId!.Value).Wait();
            }
            litMessage.Text = "<div class='alert alert-success'>Lesson marked complete!</div>";
            LoadLesson();
        }
    }
}
