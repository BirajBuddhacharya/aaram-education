using System;
using System.Linq;
using System.Web.UI.WebControls;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Admin
{
    public partial class GuestbookModerationPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireRole("Admin");
            if (!IsPostBack) LoadPending();
        }

        private void LoadPending()
        {
            using (var db = new ApplicationDbContext())
            {
                var pending = new GuestbookRepository(db).GetPendingAsync().Result.ToList();
                rptPending.DataSource = pending;
                rptPending.DataBind();
                lblNone.Visible = !pending.Any();
            }
        }

        protected void Pending_Command(object sender, RepeaterCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument?.ToString(), out int entryId)) return;

            using (var db = new ApplicationDbContext())
            {
                var repo = new GuestbookRepository(db);
                switch (e.CommandName)
                {
                    case "Approve":
                        repo.ModerateAsync(entryId, ModerationStatus.Approved, CurrentUserId!.Value).Wait();
                        break;
                    case "Reject":
                        repo.ModerateAsync(entryId, ModerationStatus.Rejected, CurrentUserId!.Value).Wait();
                        break;
                    case "Delete":
                        repo.DeleteAsync(entryId).Wait();
                        break;
                }
            }
            LoadPending();
        }
    }
}
