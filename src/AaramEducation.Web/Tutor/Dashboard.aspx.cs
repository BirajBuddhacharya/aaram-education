using System;
using System.Linq;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Tutor
{
    public partial class Dashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Tutor");
            if (!IsPostBack) LoadCourses();
        }

        private void LoadCourses()
        {
            using (var db = new ApplicationDbContext())
            {
                var repo = new CourseRepository(db);
                var courses = repo.GetByTutorAsync(CurrentUserId!.Value).Result.ToList();
                if (courses.Count == 0)
                    litEmpty.Text = "<p class='text-muted'>No courses yet. Create your first course!</p>";
                rptCourses.DataSource = courses;
                rptCourses.DataBind();
            }
        }
    }
}
