using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Data.Seed;
using AaramEducation.Infrastructure.Repositories;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// Load .env from repo root (two levels up from the Web project)
Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env"));

var builder = WebApplication.CreateBuilder(args);

// Build MySQL connection string from .env variables
var connStr = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
              $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
              $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
              $"User={Environment.GetEnvironmentVariable("DB_USER")};" +
              $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IGuestbookRepository, GuestbookRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Seed dev data
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbSeeder.SeedAsync(db);
}

Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "uploads", "avatars"));
Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "uploads", "videos"));
Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "uploads", "notes"));

app.Run();
