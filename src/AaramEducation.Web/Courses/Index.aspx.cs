using System;
using System.Collections.Generic;
using System.Linq;
using AaramEducation.Core.Entities;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Courses
{
    public partial class CoursesIndexPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var db = new ApplicationDbContext())
                {
                    var repo = new CourseRepository(db);
                    string? subject = Request.QueryString["subject"];
                    string? difficulty = Request.QueryString["difficulty"];
                    var courses = repo.GetAllPublishedAsync(subject, difficulty).Result.ToList();
                    var subjects = repo.GetAllPublishedAsync().Result.Select(c => c.Subject).Distinct().OrderBy(s => s).ToList();
                    rptCourses.DataSource = courses;
                    rptCourses.DataBind();
                    rptSubjects.DataSource = subjects;
                    rptSubjects.DataBind();
                }
            }
        }

        protected string GetSubjectOption(string? subject)
        {
            if (subject == null) return "";
            string sel = Request.QueryString["subject"] == subject ? " selected" : "";
            return $"<option value=\"{System.Web.HttpUtility.HtmlEncode(subject)}\"{sel}>{System.Web.HttpUtility.HtmlEncode(subject)}</option>";
        }
    }
}
