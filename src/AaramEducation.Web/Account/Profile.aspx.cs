using System;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Account
{
    public partial class ProfilePage : BasePage
    {
        private ApplicationDbContext _db = null!;
        private UserRepository _users = null!;

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            _db = new ApplicationDbContext();
            _users = new UserRepository(_db);

            if (!IsPostBack)
            {
                var user = _users.GetByIdAsync(CurrentUserId!.Value).Result;
                if (user == null) { Response.Redirect("~/Account/Login.aspx"); return; }
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtEmail.Text = user.Email;

                if (Session["ProfileSuccess"] != null)
                {
                    lblSuccess.Text = Session["ProfileSuccess"]!.ToString();
                    lblSuccess.Visible = true;
                    Session.Remove("ProfileSuccess");
                }
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            var user = _users.GetByIdAsync(CurrentUserId!.Value).Result;
            if (user == null) { Response.Redirect("~/Account/Login.aspx"); return; }

            string newEmail = txtEmail.Text.Trim();
            if (!user.Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase)
                && _users.EmailExistsAsync(newEmail).Result)
            {
                lblError.Text = "That email is already in use.";
                lblError.Visible = true;
                return;
            }

            user.FirstName = txtFirstName.Text.Trim();
            user.LastName = txtLastName.Text.Trim();
            user.Email = newEmail;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(txtNewPassword.Text);

            _users.UpdateAsync(user).Wait();
            AuthHelper.SignOut();
            AuthHelper.SignIn(user.UserId, user.Role.ToString(), user.FirstName, user.LastName, false);
            Session["ProfileSuccess"] = "Profile updated successfully.";
            Response.Redirect("~/Account/Profile.aspx");
        }

        protected override void OnUnload(EventArgs e) { _db?.Dispose(); base.OnUnload(e); }
    }
}
