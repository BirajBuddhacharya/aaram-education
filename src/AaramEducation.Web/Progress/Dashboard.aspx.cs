using System;
using System.Linq;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Progress
{
    public partial class Dashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (!IsPostBack) LoadDashboard();
        }

        private void LoadDashboard()
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(CurrentUserId!.Value);
                if (user == null) return;

                litXp.Text = user.TotalXpPoints.ToString();
                litStreak.Text = user.CurrentStreakDays.ToString();

                var badgeRepo = new BadgeRepository(db);
                litBadges.Text = badgeRepo.GetUserBadgesAsync(CurrentUserId.Value).Result.Count().ToString();

                var enrollmentRepo = new EnrollmentRepository(db);

                var enrollments = enrollmentRepo.GetByStudentAsync(CurrentUserId.Value).Result
                    .Where(e2 => e2.EnrollmentStatus == EnrollmentStatus.Active)
                    .ToList();

                if (enrollments.Count == 0)
                {
                    litNoCourses.Text = "<p class='text-muted'>No courses enrolled yet. <a href='~/Courses/Index.aspx'>Browse courses</a>.</p>";
                    return;
                }

                var items = enrollments.Select(enr => new
                {
                    enr.CourseId,
                    enr.Course.CourseName,
                    enr.Course.Subject,
                    PercentComplete = enr.CourseProgress?.PercentComplete ?? 0f,
                    LessonsCompleted = enr.CourseProgress?.LessonsCompleted ?? 0
                }).OrderByDescending(x => x.PercentComplete).ToList();

                rptCourses.DataSource = items;
                rptCourses.DataBind();
            }
        }
    }
}
