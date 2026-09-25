using System;
using System.Web.UI;

namespace AaramEducation.Web
{
    public partial class SiteMaster : MasterPage
    {
        protected bool IsAuthenticated { get; private set; }
        protected string? CurrentUserRole { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsAuthenticated = Helpers.AuthHelper.IsAuthenticated();
            CurrentUserRole = Helpers.AuthHelper.GetCurrentUserRole();
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Helpers.AuthHelper.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
