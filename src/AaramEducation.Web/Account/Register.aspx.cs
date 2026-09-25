using System;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Account
{
    public partial class RegisterPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsAuthenticated)
                Response.Redirect("~/Default.aspx");
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            using (var db = new ApplicationDbContext())
            {
                var repo = new UserRepository(db);

                if (repo.EmailExistsAsync(txtEmail.Text.Trim()).Result)
                {
                    lblError.Text = "An account with this email already exists.";
                    lblError.Visible = true;
                    return;
                }

                if (!Enum.TryParse<UserRole>(ddlRole.SelectedValue, out var role) || role == UserRole.Admin)
                {
                    lblError.Text = "Invalid role selection.";
                    lblError.Visible = true;
                    return;
                }

                var user = new User
                {
                    Email = txtEmail.Text.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Role = role,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                repo.CreateAsync(user).Wait();
                AuthHelper.SignIn(user.UserId, user.Role.ToString(), user.FirstName, user.LastName, false);
                Response.Redirect("~/Courses/Index.aspx");
            }
        }
    }
}
