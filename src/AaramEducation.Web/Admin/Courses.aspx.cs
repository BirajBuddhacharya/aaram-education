using System;
using System.Linq;
using System.Web.UI.WebControls;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Admin
{
    public partial class CoursesAdminPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Admin");
            if (!IsPostBack) LoadCourses();
        }

        private void LoadCourses()
        {
            using (var db = new ApplicationDbContext())
            {
                // Load all courses with tutor and enrollment count
                var courses = db.Courses
                    .Include("CreatedBy")
                    .Include("Enrollments")
                    .OrderBy(c => c.CourseName)
                    .ToList();
                rptCourses.DataSource = courses;
                rptCourses.DataBind();
            }
        }

        protected void Courses_Command(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Toggle") return;
            if (!int.TryParse(e.CommandArgument?.ToString(), out int courseId)) return;

            using (var db = new ApplicationDbContext())
            {
                var course = db.Courses.Find(courseId);
                if (course == null) return;
                new CourseRepository(db).PublishAsync(courseId, !course.IsPublished).Wait();
            }
            LoadCourses();
        }
    }
}
