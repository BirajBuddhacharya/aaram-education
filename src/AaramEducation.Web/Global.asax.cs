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
            // IIS's AspNetAppInitializationFailureModule swallows startup exceptions,
            // so record them somewhere readable before rethrowing.
            try
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
            catch (Exception ex)
            {
                LogStartupFailure(ex);
                throw;
            }
        }

        private static void LogStartupFailure(Exception ex)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Application_Start failed at " + DateTime.Now.ToString("u"));
            sb.AppendLine();

            for (Exception? cur = ex; cur != null; cur = cur.InnerException)
            {
                sb.AppendLine(cur.GetType().FullName + ": " + cur.Message);
                sb.AppendLine(cur.StackTrace);

                // Missing or mismatched assemblies only show up in LoaderExceptions.
                if (cur is System.Reflection.ReflectionTypeLoadException tle)
                    foreach (var le in tle.LoaderExceptions)
                        sb.AppendLine("  loader: " + le.GetType().Name + ": " + le.Message);

                sb.AppendLine(new string('-', 70));
            }

            try
            {
                System.IO.File.WriteAllText(
                    System.IO.Path.Combine(System.IO.Path.GetTempPath(), "aaram-startup-error.log"),
                    sb.ToString());
            }
            catch { }
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
