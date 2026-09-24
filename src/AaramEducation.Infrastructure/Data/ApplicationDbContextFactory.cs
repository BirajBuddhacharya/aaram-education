using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AaramEducation.Infrastructure.Data;

/// <summary>
/// Used only by `dotnet ef` (migrations, bundle). Bypasses Program.cs so
/// design-time tooling never needs a live DB connection to run — a fixed
/// server version replaces ServerVersion.AutoDetect, which does connect.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;Database=aaram_education;User=root;Password=password;",
            new MySqlServerVersion(new Version(11, 4, 0)));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
