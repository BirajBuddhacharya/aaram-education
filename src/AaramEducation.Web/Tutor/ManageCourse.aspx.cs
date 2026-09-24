using System;
using System.Web.UI.WebControls;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Tutor
{
    public partial class ManageCourse : BasePage
    {
        protected int CourseId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Tutor");
            if (!IsPostBack) LoadCourse();
        }

        private void LoadCourse()
        {
            using (var db = new ApplicationDbContext())
            {
                var repo = new CourseRepository(db);
                var course = repo.GetWithModulesAndLessonsAsync(CourseId).Result;
                if (course == null || course.CreatedByUserId != CurrentUserId) { Response.Redirect("~/Tutor/Dashboard.aspx"); return; }
                litCourseName.Text = System.Web.HttpUtility.HtmlEncode(course.CourseName);
                lnkNewModule.NavigateUrl = ResolveUrl("~/Tutor/ModuleForm.aspx?courseId=" + CourseId + "&id=0");
                rptModules.DataSource = course.Modules;
                rptModules.DataBind();
            }
        }

        protected void rptModules_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditModule")
                Response.Redirect("~/Tutor/ModuleForm.aspx?courseId=" + CourseId + "&id=" + id);
            else if (e.CommandName == "DeleteModule")
            {
                using (var db = new ApplicationDbContext())
                {
                    var module = db.Modules.Find(id);
                    if (module != null && module.CourseId == CourseId) { db.Modules.Remove(module); db.SaveChanges(); }
                }
                litMessage.Text = "<div class='alert alert-success'>Module deleted.</div>";
                LoadCourse();
            }
            else if (e.CommandName == "AddLesson")
                Response.Redirect("~/Tutor/LessonForm.aspx?moduleId=" + id + "&id=0");
        }

        protected void rptLessons_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int lessonId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditLesson")
                Response.Redirect("~/Tutor/LessonForm.aspx?id=" + lessonId);
            else if (e.CommandName == "DeleteLesson")
            {
                using (var db = new ApplicationDbContext())
                {
                    var lessonRepo = new LessonRepository(db);
                    lessonRepo.DeleteAsync(lessonId).Wait();
                }
                litMessage.Text = "<div class='alert alert-success'>Lesson deleted.</div>";
                LoadCourse();
            }
        }
    }
}
