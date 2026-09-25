using System;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Admin
{
    public partial class UsersPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Admin");
            if (!IsPostBack)
            {
                using (var db = new ApplicationDbContext())
                {
                    var users = new UserRepository(db).GetAllAsync().Result;
                    rptUsers.DataSource = users;
                    rptUsers.DataBind();
                }
            }
        }
    }
}
