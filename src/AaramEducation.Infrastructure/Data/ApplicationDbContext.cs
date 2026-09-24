using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<StudyNote> StudyNotes => Set<StudyNote>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<CourseProgress> CourseProgresses => Set<CourseProgress>();
    public DbSet<ModuleProgress> ModuleProgresses => Set<ModuleProgress>();
    public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<QuestionResponse> QuestionResponses => Set<QuestionResponse>();
    public DbSet<GuestbookEntry> GuestbookEntries => Set<GuestbookEntry>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<UserBadge> UserBadges => Set<UserBadge>();
    public DbSet<DailyActivityLog> DailyActivityLogs => Set<DailyActivityLog>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // ── USER ──────────────────────────────────────────────────────────
        mb.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(256).IsRequired();
            e.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            e.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            e.Property(u => u.Role).HasConversion<string>();
        });

        // ── COURSE ────────────────────────────────────────────────────────
        mb.Entity<Course>(e =>
        {
            e.HasKey(c => c.CourseId);
            e.Property(c => c.CourseName).HasMaxLength(200).IsRequired();
            e.Property(c => c.Subject).HasMaxLength(100).IsRequired();
            e.Property(c => c.DifficultyLevel).HasMaxLength(50).IsRequired();
            e.HasOne(c => c.CreatedBy)
             .WithMany()
             .HasForeignKey(c => c.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── MODULE ────────────────────────────────────────────────────────
        mb.Entity<Module>(e =>
        {
            e.HasKey(m => m.ModuleId);
            e.Property(m => m.ModuleName).HasMaxLength(200).IsRequired();
            e.HasOne(m => m.Course)
             .WithMany(c => c.Modules)
             .HasForeignKey(m => m.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── LESSON ────────────────────────────────────────────────────────
        mb.Entity<Lesson>(e =>
        {
            e.HasKey(l => l.LessonId);
            e.Property(l => l.LessonTitle).HasMaxLength(200).IsRequired();
            e.HasOne(l => l.Module)
             .WithMany(m => m.Lessons)
             .HasForeignKey(l => l.ModuleId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── VIDEO ─────────────────────────────────────────────────────────
        mb.Entity<Video>(e =>
        {
            e.HasKey(v => v.VideoId);
            e.Property(v => v.VideoTitle).HasMaxLength(200).IsRequired();
            e.HasOne(v => v.Lesson)
             .WithMany(l => l.Videos)
             .HasForeignKey(v => v.LessonId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── STUDY_NOTE ────────────────────────────────────────────────────
        mb.Entity<StudyNote>(e =>
        {
            e.HasKey(n => n.NoteId);
            e.Property(n => n.NoteTitle).HasMaxLength(200).IsRequired();
            e.HasOne(n => n.Lesson)
             .WithMany(l => l.StudyNotes)
             .HasForeignKey(n => n.LessonId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── ENROLLMENT ────────────────────────────────────────────────────
        mb.Entity<Enrollment>(e =>
        {
            e.HasKey(en => en.EnrollmentId);
            e.HasIndex(en => new { en.StudentId, en.CourseId }).IsUnique();
            e.Property(en => en.EnrollmentStatus).HasConversion<string>();
            e.HasOne(en => en.Student)
             .WithMany(u => u.Enrollments)
             .HasForeignKey(en => en.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(en => en.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(en => en.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── COURSE_PROGRESS ───────────────────────────────────────────────
        mb.Entity<CourseProgress>(e =>
        {
            e.HasKey(cp => cp.CourseProgressId);
            e.HasIndex(cp => cp.EnrollmentId).IsUnique();
            e.Property(cp => cp.Status).HasConversion<string>();
            e.HasOne(cp => cp.Enrollment)
             .WithOne(en => en.CourseProgress)
             .HasForeignKey<CourseProgress>(cp => cp.EnrollmentId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── MODULE_PROGRESS ───────────────────────────────────────────────
        mb.Entity<ModuleProgress>(e =>
        {
            e.HasKey(mp => mp.ModuleProgressId);
            e.HasIndex(mp => new { mp.StudentId, mp.ModuleId }).IsUnique();
            e.Property(mp => mp.Status).HasConversion<string>();
            e.HasOne(mp => mp.Student)
             .WithMany(u => u.ModuleProgresses)
             .HasForeignKey(mp => mp.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(mp => mp.Module)
             .WithMany(m => m.ModuleProgresses)
             .HasForeignKey(mp => mp.ModuleId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── LESSON_PROGRESS ───────────────────────────────────────────────
        mb.Entity<LessonProgress>(e =>
        {
            e.HasKey(lp => lp.LessonProgressId);
            e.HasIndex(lp => new { lp.StudentId, lp.LessonId }).IsUnique();
            e.Property(lp => lp.Status).HasConversion<string>();
            e.HasOne(lp => lp.Student)
             .WithMany(u => u.LessonProgresses)
             .HasForeignKey(lp => lp.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(lp => lp.Lesson)
             .WithMany(l => l.LessonProgresses)
             .HasForeignKey(lp => lp.LessonId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── QUIZ ──────────────────────────────────────────────────────────
        mb.Entity<Quiz>(e =>
        {
            e.HasKey(q => q.QuizId);
            e.Property(q => q.QuizTitle).HasMaxLength(200).IsRequired();
            e.HasOne(q => q.Lesson)
             .WithMany(l => l.Quizzes)
             .HasForeignKey(q => q.LessonId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── QUIZ_QUESTION ─────────────────────────────────────────────────
        mb.Entity<QuizQuestion>(e =>
        {
            e.HasKey(qq => qq.QuestionId);
            e.Property(qq => qq.QuestionType).HasConversion<string>();
            e.HasOne(qq => qq.Quiz)
             .WithMany(q => q.Questions)
             .HasForeignKey(qq => qq.QuizId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── ANSWER_OPTION ─────────────────────────────────────────────────
        mb.Entity<AnswerOption>(e =>
        {
            e.HasKey(ao => ao.OptionId);
            e.HasOne(ao => ao.Question)
             .WithMany(qq => qq.Options)
             .HasForeignKey(ao => ao.QuestionId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── QUIZ_ATTEMPT ──────────────────────────────────────────────────
        mb.Entity<QuizAttempt>(e =>
        {
            e.HasKey(qa => qa.AttemptId);
            e.Property(qa => qa.AttemptStatus).HasConversion<string>();
            e.HasOne(qa => qa.Student)
             .WithMany(u => u.QuizAttempts)
             .HasForeignKey(qa => qa.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(qa => qa.Quiz)
             .WithMany(q => q.Attempts)
             .HasForeignKey(qa => qa.QuizId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── QUESTION_RESPONSE ─────────────────────────────────────────────
        mb.Entity<QuestionResponse>(e =>
        {
            e.HasKey(qr => qr.ResponseId);
            e.HasIndex(qr => new { qr.AttemptId, qr.QuestionId }).IsUnique();
            e.HasOne(qr => qr.Attempt)
             .WithMany(qa => qa.Responses)
             .HasForeignKey(qr => qr.AttemptId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(qr => qr.Question)
             .WithMany(qq => qq.Responses)
             .HasForeignKey(qr => qr.QuestionId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(qr => qr.SelectedOption)
             .WithMany(ao => ao.Responses)
             .HasForeignKey(qr => qr.SelectedOptionId)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired(false);
        });

        // ── GUESTBOOK_ENTRY ───────────────────────────────────────────────
        mb.Entity<GuestbookEntry>(e =>
        {
            e.HasKey(ge => ge.EntryId);
            e.Property(ge => ge.GuestName).HasMaxLength(100).IsRequired();
            e.Property(ge => ge.GuestEmail).HasMaxLength(256);
            e.Property(ge => ge.ModerationStatus).HasConversion<string>();
            e.HasOne(ge => ge.Moderator)
             .WithMany(u => u.ModeratedEntries)
             .HasForeignKey(ge => ge.ModeratedBy)
             .OnDelete(DeleteBehavior.SetNull)
             .IsRequired(false);
        });

        // ── BADGE ─────────────────────────────────────────────────────────
        mb.Entity<Badge>(e =>
        {
            e.HasKey(b => b.BadgeId);
            e.HasIndex(b => b.BadgeName).IsUnique();
            e.Property(b => b.BadgeName).HasMaxLength(100).IsRequired();
            e.Property(b => b.TargetType).HasMaxLength(100).IsRequired();
        });

        // ── USER_BADGE ────────────────────────────────────────────────────
        mb.Entity<UserBadge>(e =>
        {
            e.HasKey(ub => ub.UserBadgeId);
            e.HasIndex(ub => new { ub.UserId, ub.BadgeId }).IsUnique();
            e.HasOne(ub => ub.User)
             .WithMany(u => u.UserBadges)
             .HasForeignKey(ub => ub.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(ub => ub.Badge)
             .WithMany(b => b.UserBadges)
             .HasForeignKey(ub => ub.BadgeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DAILY_ACTIVITY_LOG ────────────────────────────────────────────
        mb.Entity<DailyActivityLog>(e =>
        {
            e.HasKey(d => d.LogId);
            e.HasIndex(d => new { d.UserId, d.ActivityDate }).IsUnique();
            e.HasOne(d => d.User)
             .WithMany(u => u.DailyActivityLogs)
             .HasForeignKey(d => d.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── NOTIFICATION ──────────────────────────────────────────────────
        mb.Entity<Notification>(e =>
        {
            e.HasKey(n => n.NotificationId);
            e.Property(n => n.Title).HasMaxLength(200).IsRequired();
            e.Property(n => n.Message).HasMaxLength(1000).IsRequired();
            e.HasOne(n => n.User)
             .WithMany()
             .HasForeignKey(n => n.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(n => new { n.UserId, n.IsRead });
        });
    }
}
