using System;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Courses
{
    public partial class MyEnrollmentsPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            using (var db = new ApplicationDbContext())
            {
                var repo = new EnrollmentRepository(db);
                var enrollments = repo.GetByStudentAsync(CurrentUserId!.Value).Result;
                rptEnrollments.DataSource = enrollments;
                rptEnrollments.DataBind();
            }
        }
    }
}
