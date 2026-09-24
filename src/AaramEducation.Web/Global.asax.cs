using System;
using System.Web;
using System.Web.Security;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Data.Seed;

namespace AaramEducation.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            System.Data.Entity.Database.SetInitializer(
                new System.Data.Entity.CreateDatabaseIfNotExists<ApplicationDbContext>());

            using (var db = new ApplicationDbContext())
            {
                db.Database.Initialize(false);
                DbSeeder.Seed(db);
            }

            string uploads = Server.MapPath("~/uploads");
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(uploads, "avatars"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(uploads, "videos"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(uploads, "notes"));
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie? cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        var identity = new System.Security.Principal.GenericIdentity(ticket.Name, "Forms");
                        Context.User = new System.Security.Principal.GenericPrincipal(identity, null);
                    }
                }
                catch { }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception? ex = Server.GetLastError();
            if (ex != null)
            {
                Server.ClearError();
                Response.Redirect("~/Shared/Error.aspx");
            }
        }
    }
}
