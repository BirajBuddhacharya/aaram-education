using System;
using System.Web;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Account
{
    public partial class LoginPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsAuthenticated)
                Response.Redirect("~/Courses/Index.aspx");
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            using (var db = new ApplicationDbContext())
            {
                var repo = new UserRepository(db);
                var user = repo.GetByEmailAsync(txtEmail.Text.Trim()).Result;

                if (user == null || !BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.PasswordHash))
                {
                    lblError.Text = "Invalid email or password.";
                    lblError.Visible = true;
                    return;
                }

                AuthHelper.SignIn(user.UserId, user.Role.ToString(), user.FirstName, user.LastName, chkRemember.Checked);

                string returnUrl = Request.QueryString["returnUrl"];
                Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/Courses/Index.aspx");
            }
        }
    }
}
