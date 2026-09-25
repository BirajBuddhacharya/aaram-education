using System;
using AaramEducation.Core.Entities;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Tutor
{
    public partial class LessonForm : BasePage
    {
        private int LessonId => int.TryParse(hdnLessonId.Value, out int id) ? id : 0;
        private int ModuleId => int.TryParse(hdnModuleId.Value, out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Tutor");
            if (!IsPostBack)
            {
                int lessonId = int.TryParse(Request.QueryString["id"], out int lid) ? lid : 0;
                int moduleId = int.TryParse(Request.QueryString["moduleId"], out int mid) ? mid : 0;
                hdnLessonId.Value = lessonId.ToString();

                if (lessonId == 0)
                {
                    litHeading.Text = "New Lesson";
                    hdnModuleId.Value = moduleId.ToString();
                    SetCancelUrl(moduleId);
                }
                else
                {
                    litHeading.Text = "Edit Lesson";
                    using (var db = new ApplicationDbContext())
                    {
                        var repo = new LessonRepository(db);
                        var lesson = repo.GetWithContentAsync(lessonId).Result;
                        if (lesson == null) { Response.Redirect("~/Tutor/Dashboard.aspx"); return; }
                        hdnModuleId.Value = lesson.ModuleId.ToString();
                        SetCancelUrl(lesson.ModuleId);
                        txtTitle.Text = lesson.LessonTitle;
                        txtDescription.Text = lesson.LessonDescription;
                        txtOrder.Text = lesson.SequenceOrder.ToString();
                        chkFreeSample.Checked = lesson.IsFreeSample;

                        var video = repo.GetVideoAsync(lessonId).Result;
                        if (video != null)
                        {
                            hdnVideoId.Value = video.VideoId.ToString();
                            txtVideoTitle.Text = video.VideoTitle;
                            txtVideoUrl.Text = video.VideoUrl;
                        }

                        if (lesson.StudyNotes.Count > 0)
                        {
                            var note = lesson.StudyNotes.GetEnumerator();
                            note.MoveNext();
                            var n = note.Current;
                            hdnNoteId.Value = n.NoteId.ToString();
                            txtNoteTitle.Text = n.NoteTitle;
                            txtNoteContent.Text = n.NoteContent;
                        }
                    }
                }
            }
        }

        private void SetCancelUrl(int moduleId)
        {
            using (var db = new ApplicationDbContext())
            {
                var m = db.Modules.Find(moduleId);
                lnkCancel.NavigateUrl = m != null
                    ? ResolveUrl("~/Tutor/ManageCourse.aspx?id=" + m.CourseId)
                    : ResolveUrl("~/Tutor/Dashboard.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                litMessage.Text = "<div class='alert alert-danger'>Title is required.</div>";
                return;
            }
            int order = int.TryParse(txtOrder.Text, out int o) ? o : 1;
            using (var db = new ApplicationDbContext())
            {
                var repo = new LessonRepository(db);
                Lesson lesson;
                if (LessonId == 0)
                {
                    lesson = repo.CreateAsync(new Lesson
                    {
                        ModuleId = ModuleId,
                        LessonTitle = txtTitle.Text.Trim(),
                        LessonDescription = txtDescription.Text.Trim(),
                        SequenceOrder = order,
                        IsFreeSample = chkFreeSample.Checked,
                        CreatedAt = DateTime.UtcNow
                    }).Result;
                }
                else
                {
                    lesson = repo.GetByIdAsync(LessonId).Result!;
                    lesson.LessonTitle = txtTitle.Text.Trim();
                    lesson.LessonDescription = txtDescription.Text.Trim();
                    lesson.SequenceOrder = order;
                    lesson.IsFreeSample = chkFreeSample.Checked;
                    repo.UpdateAsync(lesson).Wait();
                }

                if (!string.IsNullOrWhiteSpace(txtVideoTitle.Text) || !string.IsNullOrWhiteSpace(txtVideoUrl.Text))
                {
                    int videoId = int.TryParse(hdnVideoId.Value, out int vid) ? vid : 0;
                    repo.SaveVideoAsync(new Video
                    {
                        VideoId = videoId,
                        LessonId = lesson.LessonId,
                        VideoTitle = txtVideoTitle.Text.Trim(),
                        VideoUrl = txtVideoUrl.Text.Trim(),
                        UploadedAt = DateTime.UtcNow
                    }).Wait();
                }

                if (!string.IsNullOrWhiteSpace(txtNoteTitle.Text) || !string.IsNullOrWhiteSpace(txtNoteContent.Text))
                {
                    int noteId = int.TryParse(hdnNoteId.Value, out int nid) ? nid : 0;
                    repo.SaveStudyNoteAsync(new StudyNote
                    {
                        NoteId = noteId,
                        LessonId = lesson.LessonId,
                        NoteTitle = txtNoteTitle.Text.Trim(),
                        NoteContent = txtNoteContent.Text.Trim(),
                        CreatedAt = DateTime.UtcNow
                    }).Wait();
                }
            }

            int moduleId = ModuleId;
            using (var db = new ApplicationDbContext())
            {
                var m = db.Modules.Find(moduleId);
                if (m != null) Response.Redirect("~/Tutor/ManageCourse.aspx?id=" + m.CourseId);
                else Response.Redirect("~/Tutor/Dashboard.aspx");
            }
        }
    }
}
