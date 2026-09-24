using System;
using System.Data.Entity;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Admin
{
    public partial class Dashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Admin");
            if (!IsPostBack)
            {
                using (var db = new ApplicationDbContext())
                {
                    litUsers.Text = db.Users.CountAsync().Result.ToString();
                    litCourses.Text = db.Courses.CountAsync(c => c.IsPublished).Result.ToString();
                    litEnrollments.Text = db.Enrollments.CountAsync().Result.ToString();
                    litPending.Text = db.GuestbookEntries
                        .CountAsync(g => g.ModerationStatus == ModerationStatus.Pending).Result.ToString();
                }
            }
        }
    }
}
