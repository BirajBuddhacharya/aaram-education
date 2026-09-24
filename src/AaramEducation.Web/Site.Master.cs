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
            IsAuthenticated = App_Code.AuthHelper.IsAuthenticated();
            CurrentUserRole = App_Code.AuthHelper.GetCurrentUserRole();
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            App_Code.AuthHelper.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
