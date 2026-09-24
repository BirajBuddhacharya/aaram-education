using System;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Notifications
{
    public partial class NotificationsIndexPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            using (var db = new ApplicationDbContext())
            {
                var repo = new NotificationRepository(db);
                var notifications = repo.GetRecentAsync(CurrentUserId!.Value, 50).Result;
                rptNotifications.DataSource = notifications;
                rptNotifications.DataBind();
            }
        }

        protected void MarkAllRead_Click(object sender, EventArgs e)
        {
            using (var db = new ApplicationDbContext())
                new NotificationRepository(db).MarkAllReadAsync(CurrentUserId!.Value).Wait();
            Response.Redirect("~/Notifications/Index.aspx");
        }
    }
}
