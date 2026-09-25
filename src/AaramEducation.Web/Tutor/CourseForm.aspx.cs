using System;
using AaramEducation.Core.Entities;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Tutor
{
    public partial class CourseForm : BasePage
    {
        private int CourseId => int.TryParse(hdnCourseId.Value, out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Tutor");
            if (!IsPostBack)
            {
                int id = int.TryParse(Request.QueryString["id"], out int qid) ? qid : 0;
                hdnCourseId.Value = id.ToString();
                if (id == 0)
                {
                    litHeading.Text = "New Course";
                }
                else
                {
                    litHeading.Text = "Edit Course";
                    LoadCourse(id);
                }
            }
        }

        private void LoadCourse(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var repo = new CourseRepository(db);
                var course = repo.GetByIdAsync(id).Result;
                if (course == null || course.CreatedByUserId != CurrentUserId) { Response.Redirect("~/Tutor/Dashboard.aspx"); return; }
                txtName.Text = course.CourseName;
                txtDescription.Text = course.CourseDescription;
                txtSubject.Text = course.Subject;
                ddlDifficulty.SelectedValue = course.DifficultyLevel;
                chkPublish.Checked = course.IsPublished;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                litMessage.Text = "<div class='alert alert-danger'>Course name is required.</div>";
                return;
            }
            using (var db = new ApplicationDbContext())
            {
                var repo = new CourseRepository(db);
                if (CourseId == 0)
                {
                    var course = new Course
                    {
                        CourseName = txtName.Text.Trim(),
                        CourseDescription = txtDescription.Text.Trim(),
                        Subject = txtSubject.Text.Trim(),
                        DifficultyLevel = ddlDifficulty.SelectedValue,
                        IsPublished = chkPublish.Checked,
                        CreatedByUserId = CurrentUserId!.Value,
                        CreatedAt = DateTime.UtcNow
                    };
                    var created = repo.CreateAsync(course).Result;
                    Response.Redirect("~/Tutor/ManageCourse.aspx?id=" + created.CourseId);
                }
                else
                {
                    var course = repo.GetByIdAsync(CourseId).Result;
                    if (course == null || course.CreatedByUserId != CurrentUserId) { Response.Redirect("~/Tutor/Dashboard.aspx"); return; }
                    course.CourseName = txtName.Text.Trim();
                    course.CourseDescription = txtDescription.Text.Trim();
                    course.Subject = txtSubject.Text.Trim();
                    course.DifficultyLevel = ddlDifficulty.SelectedValue;
                    course.IsPublished = chkPublish.Checked;
                    repo.UpdateAsync(course).Wait();
                    litMessage.Text = "<div class='alert alert-success'>Course saved.</div>";
                }
            }
        }
    }
}
