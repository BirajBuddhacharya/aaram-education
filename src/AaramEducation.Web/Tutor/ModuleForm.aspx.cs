using System;
using AaramEducation.Core.Entities;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Tutor
{
    public partial class ModuleForm : BasePage
    {
        private int ModuleId => int.TryParse(hdnModuleId.Value, out int id) ? id : 0;
        private int CourseId => int.TryParse(hdnCourseId.Value, out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Tutor");
            if (!IsPostBack)
            {
                int moduleId = int.TryParse(Request.QueryString["id"], out int mid) ? mid : 0;
                int courseId = int.TryParse(Request.QueryString["courseId"], out int cid) ? cid : 0;
                hdnModuleId.Value = moduleId.ToString();
                hdnCourseId.Value = courseId.ToString();
                lnkCancel.NavigateUrl = ResolveUrl("~/Tutor/ManageCourse.aspx?id=" + courseId);
                if (moduleId == 0)
                {
                    litHeading.Text = "New Module";
                }
                else
                {
                    litHeading.Text = "Edit Module";
                    using (var db = new ApplicationDbContext())
                    {
                        var m = db.Modules.Find(moduleId);
                        if (m == null) { Response.Redirect("~/Tutor/Dashboard.aspx"); return; }
                        txtName.Text = m.ModuleName;
                        txtDescription.Text = m.ModuleDescription;
                        txtOrder.Text = m.SequenceOrder.ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                litMessage.Text = "<div class='alert alert-danger'>Module name is required.</div>";
                return;
            }
            int order = int.TryParse(txtOrder.Text, out int o) ? o : 1;
            using (var db = new ApplicationDbContext())
            {
                if (ModuleId == 0)
                {
                    db.Modules.Add(new Module
                    {
                        CourseId = CourseId,
                        ModuleName = txtName.Text.Trim(),
                        ModuleDescription = txtDescription.Text.Trim(),
                        SequenceOrder = order,
                        CreatedAt = DateTime.UtcNow
                    });
                    db.SaveChanges();
                }
                else
                {
                    var m = db.Modules.Find(ModuleId);
                    if (m != null)
                    {
                        m.ModuleName = txtName.Text.Trim();
                        m.ModuleDescription = txtDescription.Text.Trim();
                        m.SequenceOrder = order;
                        db.SaveChanges();
                    }
                }
            }
            Response.Redirect("~/Tutor/ManageCourse.aspx?id=" + CourseId);
        }
    }
}
