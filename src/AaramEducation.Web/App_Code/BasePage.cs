using System.Web.UI;

namespace AaramEducation.Web.App_Code
{
    public class BasePage : Page
    {
        protected int? CurrentUserId => AuthHelper.GetCurrentUserId();
        protected string? CurrentUserRole => AuthHelper.GetCurrentUserRole();
        protected bool IsAuthenticated => AuthHelper.IsAuthenticated();

        protected void RequireAuth()
        {
            if (!IsAuthenticated)
                Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
        }

        protected void RequireRole(string role)
        {
            RequireAuth();
            if (CurrentUserRole != role)
                Response.Redirect("~/Account/Login.aspx");
        }
    }
}
