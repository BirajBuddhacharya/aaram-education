using System;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Courses
{
    public partial class CourseDetailsPage : BasePage
    {
        private Course? _course;
        private int _courseId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["id"], out _courseId))
            { ShowNotFound(); return; }

            using (var db = new ApplicationDbContext())
            {
                var courseRepo = new CourseRepository(db);
                var enrollRepo = new EnrollmentRepository(db);
                _course = courseRepo.GetWithModulesAndLessonsAsync(_courseId).Result;
                if (_course == null) { ShowNotFound(); return; }

                litCourseName.Text = System.Web.HttpUtility.HtmlEncode(_course.CourseName);
                litSubject.Text = System.Web.HttpUtility.HtmlEncode(_course.Subject);
                litDifficulty.Text = System.Web.HttpUtility.HtmlEncode(_course.DifficultyLevel);
                litTutor.Text = System.Web.HttpUtility.HtmlEncode(
                    $"{_course.CreatedBy?.FirstName} {_course.CreatedBy?.LastName}".Trim());
                litDescription.Text = System.Web.HttpUtility.HtmlEncode(_course.CourseDescription ?? "");

                rptModules.DataSource = _course.Modules;
                rptModules.DataBind();

                if (IsAuthenticated)
                {
                    var enrollment = enrollRepo.GetAsync(CurrentUserId!.Value, _courseId).Result;
                    if (enrollment != null && enrollment.EnrollmentStatus == EnrollmentStatus.Active)
                    {
                        pnlEnrolled.Visible = true;
                        litProgress.Text = ((int)(enrollment.CourseProgress?.PercentComplete ?? 0)).ToString();
                    }
                    else pnlNotEnrolled.Visible = true;
                }
                else pnlLoginToEnroll.Visible = true;
            }
        }

        protected void Enroll_Click(object sender, EventArgs e)
        {
            RequireAuth();
            using (var db = new ApplicationDbContext())
                new EnrollmentRepository(db).EnrollAsync(CurrentUserId!.Value, _courseId).Wait();
            Response.Redirect(Request.RawUrl);
        }

        protected void Drop_Click(object sender, EventArgs e)
        {
            RequireAuth();
            using (var db = new ApplicationDbContext())
                new EnrollmentRepository(db).DropAsync(CurrentUserId!.Value, _courseId).Wait();
            Response.Redirect("~/Courses/MyEnrollments.aspx");
        }

        private void ShowNotFound()
        {
            lblNotFound.Visible = true;
            courseContent.Visible = false;
        }
    }
}
