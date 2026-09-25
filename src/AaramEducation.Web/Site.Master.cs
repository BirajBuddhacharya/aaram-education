using System;
using System.Linq;
using System.Web.UI;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web
{
    public partial class SiteMaster : MasterPage
    {
        protected bool IsAuthenticated { get; private set; }
        protected string CurrentUserRole { get; private set; } = "Student";
        protected string UserDisplayName { get; private set; } = string.Empty;
        protected string UserInitials { get; private set; } = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            IsAuthenticated = AuthHelper.IsAuthenticated();
            CurrentUserRole = AuthHelper.GetCurrentUserRole() ?? "Student";
            if (!IsAuthenticated) return;

            string full = AuthHelper.GetCurrentUserFullName() ?? string.Empty;
            string first = AuthHelper.GetCurrentUserFirstName() ?? string.Empty;
            UserDisplayName = first.Length > 0 ? first : full;

            string[] parts = full.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            UserInitials = (parts.Length >= 2
                ? string.Concat(parts[0][0], parts[1][0])
                : UserDisplayName.Substring(0, Math.Min(2, UserDisplayName.Length))).ToUpperInvariant();

            if (!Page.IsPostBack) BindNotifications();
        }

        private void BindNotifications()
        {
            int? userId = AuthHelper.GetCurrentUserId();
            if (userId == null) return;

            using (var db = new ApplicationDbContext())
            {
                var repo = new NotificationRepository(db);
                var recent = repo.GetRecentAsync(userId.Value, 5).Result.ToList();
                int unread = repo.GetUnreadCountAsync(userId.Value).Result;

                rptNotifications.DataSource = recent;
                rptNotifications.DataBind();
                pnlNotifyEmpty.Visible = recent.Count == 0;
                pnlNotifyList.Visible = recent.Count > 0;

                pnlUnreadBadge.Visible = unread > 0;
                litUnreadCount.Text = unread > 9 ? "9+" : unread.ToString();
            }
        }

        // Marks a top-level section active in the navbar, mirroring the MVC layout's
        // controller check.
        protected string NavActive(string section)
        {
            string path = Request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
            return path.StartsWith("~/" + section + "/", StringComparison.OrdinalIgnoreCase)
                ? "active" : string.Empty;
        }

        protected string NavActivePage(string relativePath)
        {
            string path = Request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
            return string.Equals(path, "~/" + relativePath, StringComparison.OrdinalIgnoreCase)
                ? "active" : string.Empty;
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            AuthHelper.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
