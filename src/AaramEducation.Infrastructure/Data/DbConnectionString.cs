using System;
using System.IO;

namespace AaramEducation.Infrastructure.Data
{
    // Connection details come from DB_* environment variables, loaded from a .env at
    // the repository root when present, so credentials stay out of source control.
    // Falls back to the DefaultConnection entry in web.config when they are absent.
    internal static class DbConnectionString
    {
        private static readonly Lazy<string> Resolved = new Lazy<string>(Build);

        public static string Value => Resolved.Value;

        private static string Build()
        {
            LoadDotEnv();

            string host = Read("DB_HOST");
            string name = Read("DB_NAME");
            string user = Read("DB_USER");
            if (host.Length == 0 || name.Length == 0 || user.Length == 0)
                return "name=DefaultConnection";

            string port = Read("DB_PORT");
            if (port.Length == 0) port = "3306";

            // Managed providers require TLS; set DB_SSLMODE=none for a plain local server.
            string ssl = Read("DB_SSLMODE");
            if (ssl.Length == 0) ssl = "Required";

            return "server=" + host + ";port=" + port + ";database=" + name
                 + ";user=" + user + ";password=" + Read("DB_PASSWORD")
                 + ";SslMode=" + ssl + ";AllowPublicKeyRetrieval=true;CharSet=utf8;";
        }

        private static string Read(string key) =>
            Environment.GetEnvironmentVariable(key) ?? string.Empty;

        private static void LoadDotEnv()
        {
            string path = FindDotEnv();
            if (path == null) return;

            foreach (string raw in File.ReadAllLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line[0] == '#') continue;

                int eq = line.IndexOf('=');
                if (eq <= 0) continue;

                string key = line.Substring(0, eq).Trim();
                string value = line.Substring(eq + 1).Trim();
                if (value.Length > 1 && value[0] == value[value.Length - 1] &&
                    (value[0] == '"' || value[0] == '\''))
                    value = value.Substring(1, value.Length - 2);

                // A real environment variable always wins over the file.
                if (Environment.GetEnvironmentVariable(key) == null)
                    Environment.SetEnvironmentVariable(key, value);
            }
        }

        // bin/ sits several levels below the repository root, so walk upwards.
        private static string FindDotEnv()
        {
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            for (int i = 0; i < 8 && dir != null; i++, dir = dir.Parent)
            {
                string candidate = Path.Combine(dir.FullName, ".env");
                if (File.Exists(candidate)) return candidate;
            }
            return null;
        }
    }
}
