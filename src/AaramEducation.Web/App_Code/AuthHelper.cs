using System;
using System.Web;
using System.Web.Security;

namespace AaramEducation.Web.App_Code
{
    // Ticket UserData: "{userId}|{role}|{firstName}|{lastName}"
    public static class AuthHelper
    {
        public static void SignIn(int userId, string role, string firstName, string lastName, bool rememberMe)
        {
            string userData = $"{userId}|{role}|{firstName}|{lastName}";
            var ticket = new FormsAuthenticationTicket(
                1, userId.ToString(), DateTime.Now,
                DateTime.Now.AddDays(rememberMe ? 7 : 1),
                rememberMe, userData, FormsAuthentication.FormsCookiePath);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted) { HttpOnly = true };
            if (rememberMe) cookie.Expires = DateTime.Now.AddDays(7);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SignOut() => FormsAuthentication.SignOut();

        public static bool IsAuthenticated() => GetTicket() != null;

        public static int? GetCurrentUserId()
        {
            var parts = GetParts();
            return parts != null && parts.Length >= 1 && int.TryParse(parts[0], out int id) ? id : (int?)null;
        }

        public static string? GetCurrentUserRole()
        {
            var parts = GetParts();
            return parts?.Length >= 2 ? parts[1] : null;
        }

        public static string? GetCurrentUserFirstName()
        {
            var parts = GetParts();
            return parts?.Length >= 3 ? parts[2] : null;
        }

        public static string? GetCurrentUserFullName()
        {
            var parts = GetParts();
            if (parts == null) return null;
            string first = parts.Length >= 3 ? parts[2] : "";
            string last = parts.Length >= 4 ? parts[3] : "";
            return $"{first} {last}".Trim();
        }

        private static string[]? GetParts() => GetTicket()?.UserData?.Split('|');

        private static FormsAuthenticationTicket? GetTicket()
        {
            try
            {
                var cookie = HttpContext.Current?.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null) return null;
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                return ticket?.Expired == false ? ticket : null;
            }
            catch { return null; }
        }
    }
}
