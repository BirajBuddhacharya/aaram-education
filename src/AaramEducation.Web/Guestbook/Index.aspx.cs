using System;
using System.Web;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Guestbook
{
    public partial class GuestbookIndexPage : BasePage
    {
        private const int PageSize = 10;
        protected int CurrentPage { get; private set; } = 1;
        protected int TotalPages { get; private set; } = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPage = Math.Max(1, int.TryParse(Request.QueryString["page"], out int p) ? p : 1);

            using (var db = new ApplicationDbContext())
            {
                var repo = new GuestbookRepository(db);
                int total = (int)repo.GetApprovedCountAsync().Result;
                TotalPages = Math.Max(1, (int)Math.Ceiling((double)total / PageSize));
                litCount.Text = total.ToString();

                var entries = repo.GetApprovedAsync(CurrentPage, PageSize).Result;
                rptEntries.DataSource = entries;
                rptEntries.DataBind();
            }

            if (!IsPostBack)
            {
                if (IsAuthenticated)
                    txtName.Text = AuthHelper.GetCurrentUserFullName();

                if (Session["GuestSuccess"] != null)
                {
                    lblSuccess.Text = Session["GuestSuccess"]!.ToString();
                    lblSuccess.Visible = true;
                    Session.Remove("GuestSuccess");
                }
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            using (var db = new ApplicationDbContext())
            {
                var repo = new GuestbookRepository(db);
                repo.SubmitAsync(new GuestbookEntry
                {
                    GuestName = HttpUtility.HtmlEncode(txtName.Text.Trim()),
                    GuestEmail = txtEmail.Text.Trim(),
                    Message = HttpUtility.HtmlEncode(txtMessage.Text.Trim()),
                    SubmittedAt = DateTime.UtcNow,
                    ModerationStatus = ModerationStatus.Pending,
                }).Wait();
            }
            Session["GuestSuccess"] = "Your message has been submitted for review. Thank you!";
            Response.Redirect("~/Guestbook/Index.aspx");
        }
    }
}
