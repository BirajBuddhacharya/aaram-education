using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using AaramEducation.Core.Entities;
using MySql.Data.Entity;

[assembly: DbConfigurationType(typeof(MySqlEFConfiguration))]

namespace AaramEducation.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DefaultConnection") { }
        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<Video> Videos { get; set; } = null!;
        public DbSet<StudyNote> StudyNotes { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<CourseProgress> CourseProgresses { get; set; } = null!;
        public DbSet<ModuleProgress> ModuleProgresses { get; set; } = null!;
        public DbSet<LessonProgress> LessonProgresses { get; set; } = null!;
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<QuizQuestion> QuizQuestions { get; set; } = null!;
        public DbSet<AnswerOption> AnswerOptions { get; set; } = null!;
        public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
        public DbSet<QuestionResponse> QuestionResponses { get; set; } = null!;
        public DbSet<GuestbookEntry> GuestbookEntries { get; set; } = null!;
        public DbSet<Badge> Badges { get; set; } = null!;
        public DbSet<UserBadge> UserBadges { get; set; } = null!;
        public DbSet<DailyActivityLog> DailyActivityLogs { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.Conventions.Remove<PluralizingTableNameConvention>();

            // USER
            mb.Entity<User>().HasKey(u => u.UserId).ToTable("Users");
            mb.Entity<User>().Property(u => u.Email).HasMaxLength(256).IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Users_Email") { IsUnique = true }));
            mb.Entity<User>().Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            mb.Entity<User>().Property(u => u.LastName).HasMaxLength(100).IsRequired();

            // COURSE
            mb.Entity<Course>().HasKey(c => c.CourseId).ToTable("Courses");
            mb.Entity<Course>().Property(c => c.CourseName).HasMaxLength(200).IsRequired();
            mb.Entity<Course>().Property(c => c.Subject).HasMaxLength(100).IsRequired();
            mb.Entity<Course>().Property(c => c.DifficultyLevel).HasMaxLength(50).IsRequired();
            mb.Entity<Course>().HasRequired(c => c.CreatedBy).WithMany().HasForeignKey(c => c.CreatedByUserId).WillCascadeOnDelete(false);

            // MODULE
            mb.Entity<Module>().HasKey(m => m.ModuleId).ToTable("Modules");
            mb.Entity<Module>().Property(m => m.ModuleName).HasMaxLength(200).IsRequired();
            mb.Entity<Module>().HasRequired(m => m.Course).WithMany(c => c.Modules).HasForeignKey(m => m.CourseId).WillCascadeOnDelete(true);

            // LESSON
            mb.Entity<Lesson>().HasKey(l => l.LessonId).ToTable("Lessons");
            mb.Entity<Lesson>().Property(l => l.LessonTitle).HasMaxLength(200).IsRequired();
            mb.Entity<Lesson>().HasRequired(l => l.Module).WithMany(m => m.Lessons).HasForeignKey(l => l.ModuleId).WillCascadeOnDelete(true);

            // VIDEO
            mb.Entity<Video>().HasKey(v => v.VideoId).ToTable("Videos");
            mb.Entity<Video>().Property(v => v.VideoTitle).HasMaxLength(200).IsRequired();
            mb.Entity<Video>().HasRequired(v => v.Lesson).WithMany(l => l.Videos).HasForeignKey(v => v.LessonId).WillCascadeOnDelete(true);

            // STUDY NOTE
            mb.Entity<StudyNote>().HasKey(n => n.NoteId).ToTable("StudyNotes");
            mb.Entity<StudyNote>().Property(n => n.NoteTitle).HasMaxLength(200).IsRequired();
            mb.Entity<StudyNote>().HasRequired(n => n.Lesson).WithMany(l => l.StudyNotes).HasForeignKey(n => n.LessonId).WillCascadeOnDelete(true);

            // ENROLLMENT
            mb.Entity<Enrollment>().HasKey(e => e.EnrollmentId).ToTable("Enrollments");
            mb.Entity<Enrollment>().Property(e => e.StudentId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_Enrollment_Student_Course", 1) { IsUnique = true } }));
            mb.Entity<Enrollment>().Property(e => e.CourseId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_Enrollment_Student_Course", 2) { IsUnique = true } }));
            mb.Entity<Enrollment>().HasRequired(e => e.Student).WithMany(u => u.Enrollments).HasForeignKey(e => e.StudentId).WillCascadeOnDelete(false);
            mb.Entity<Enrollment>().HasRequired(e => e.Course).WithMany(c => c.Enrollments).HasForeignKey(e => e.CourseId).WillCascadeOnDelete(false);

            // COURSE PROGRESS
            mb.Entity<CourseProgress>().HasKey(cp => cp.CourseProgressId).ToTable("CourseProgresses");
            mb.Entity<CourseProgress>().HasRequired(cp => cp.Enrollment).WithOptionalDependent(e => e.CourseProgress).WillCascadeOnDelete(true);

            // MODULE PROGRESS
            mb.Entity<ModuleProgress>().HasKey(mp => mp.ModuleProgressId).ToTable("ModuleProgresses");
            mb.Entity<ModuleProgress>().Property(mp => mp.StudentId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_ModuleProgress_Student_Module", 1) { IsUnique = true } }));
            mb.Entity<ModuleProgress>().Property(mp => mp.ModuleId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_ModuleProgress_Student_Module", 2) { IsUnique = true } }));
            mb.Entity<ModuleProgress>().HasRequired(mp => mp.Student).WithMany(u => u.ModuleProgresses).HasForeignKey(mp => mp.StudentId).WillCascadeOnDelete(false);
            mb.Entity<ModuleProgress>().HasRequired(mp => mp.Module).WithMany(m => m.ModuleProgresses).HasForeignKey(mp => mp.ModuleId).WillCascadeOnDelete(false);

            // LESSON PROGRESS
            mb.Entity<LessonProgress>().HasKey(lp => lp.LessonProgressId).ToTable("LessonProgresses");
            mb.Entity<LessonProgress>().Property(lp => lp.StudentId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_LessonProgress_Student_Lesson", 1) { IsUnique = true } }));
            mb.Entity<LessonProgress>().Property(lp => lp.LessonId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_LessonProgress_Student_Lesson", 2) { IsUnique = true } }));
            mb.Entity<LessonProgress>().HasRequired(lp => lp.Student).WithMany(u => u.LessonProgresses).HasForeignKey(lp => lp.StudentId).WillCascadeOnDelete(false);
            mb.Entity<LessonProgress>().HasRequired(lp => lp.Lesson).WithMany(l => l.LessonProgresses).HasForeignKey(lp => lp.LessonId).WillCascadeOnDelete(false);

            // QUIZ
            mb.Entity<Quiz>().HasKey(q => q.QuizId).ToTable("Quizzes");
            mb.Entity<Quiz>().Property(q => q.QuizTitle).HasMaxLength(200).IsRequired();
            mb.Entity<Quiz>().HasRequired(q => q.Lesson).WithMany(l => l.Quizzes).HasForeignKey(q => q.LessonId).WillCascadeOnDelete(true);

            // QUIZ QUESTION
            mb.Entity<QuizQuestion>().HasKey(qq => qq.QuestionId).ToTable("QuizQuestions");
            mb.Entity<QuizQuestion>().HasRequired(qq => qq.Quiz).WithMany(q => q.Questions).HasForeignKey(qq => qq.QuizId).WillCascadeOnDelete(true);

            // ANSWER OPTION
            mb.Entity<AnswerOption>().HasKey(ao => ao.OptionId).ToTable("AnswerOptions");
            mb.Entity<AnswerOption>().HasRequired(ao => ao.Question).WithMany(qq => qq.Options).HasForeignKey(ao => ao.QuestionId).WillCascadeOnDelete(true);

            // QUIZ ATTEMPT
            mb.Entity<QuizAttempt>().HasKey(qa => qa.AttemptId).ToTable("QuizAttempts");
            mb.Entity<QuizAttempt>().HasRequired(qa => qa.Student).WithMany(u => u.QuizAttempts).HasForeignKey(qa => qa.StudentId).WillCascadeOnDelete(false);
            mb.Entity<QuizAttempt>().HasRequired(qa => qa.Quiz).WithMany(q => q.Attempts).HasForeignKey(qa => qa.QuizId).WillCascadeOnDelete(false);

            // QUESTION RESPONSE
            mb.Entity<QuestionResponse>().HasKey(qr => qr.ResponseId).ToTable("QuestionResponses");
            mb.Entity<QuestionResponse>().Property(qr => qr.AttemptId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_QuestionResponse_Attempt_Question", 1) { IsUnique = true } }));
            mb.Entity<QuestionResponse>().Property(qr => qr.QuestionId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_QuestionResponse_Attempt_Question", 2) { IsUnique = true } }));
            mb.Entity<QuestionResponse>().HasRequired(qr => qr.Attempt).WithMany(qa => qa.Responses).HasForeignKey(qr => qr.AttemptId).WillCascadeOnDelete(true);
            mb.Entity<QuestionResponse>().HasRequired(qr => qr.Question).WithMany(qq => qq.Responses).HasForeignKey(qr => qr.QuestionId).WillCascadeOnDelete(false);
            mb.Entity<QuestionResponse>().HasOptional(qr => qr.SelectedOption).WithMany(ao => ao.Responses).HasForeignKey(qr => qr.SelectedOptionId).WillCascadeOnDelete(false);

            // GUESTBOOK ENTRY
            mb.Entity<GuestbookEntry>().HasKey(ge => ge.EntryId).ToTable("GuestbookEntries");
            mb.Entity<GuestbookEntry>().Property(ge => ge.GuestName).HasMaxLength(100).IsRequired();
            mb.Entity<GuestbookEntry>().Property(ge => ge.GuestEmail).HasMaxLength(256);
            mb.Entity<GuestbookEntry>().HasOptional(ge => ge.Moderator).WithMany(u => u.ModeratedEntries).HasForeignKey(ge => ge.ModeratedBy).WillCascadeOnDelete(false);

            // BADGE
            mb.Entity<Badge>().HasKey(b => b.BadgeId).ToTable("Badges");
            mb.Entity<Badge>().Property(b => b.BadgeName).HasMaxLength(100).IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Badge_Name") { IsUnique = true }));
            mb.Entity<Badge>().Property(b => b.TargetType).HasMaxLength(100).IsRequired();

            // USER BADGE
            mb.Entity<UserBadge>().HasKey(ub => ub.UserBadgeId).ToTable("UserBadges");
            mb.Entity<UserBadge>().Property(ub => ub.UserId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_UserBadge_User_Badge", 1) { IsUnique = true } }));
            mb.Entity<UserBadge>().Property(ub => ub.BadgeId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_UserBadge_User_Badge", 2) { IsUnique = true } }));
            mb.Entity<UserBadge>().HasRequired(ub => ub.User).WithMany(u => u.UserBadges).HasForeignKey(ub => ub.UserId).WillCascadeOnDelete(false);
            mb.Entity<UserBadge>().HasRequired(ub => ub.Badge).WithMany(b => b.UserBadges).HasForeignKey(ub => ub.BadgeId).WillCascadeOnDelete(false);

            // DAILY ACTIVITY LOG
            mb.Entity<DailyActivityLog>().HasKey(d => d.LogId).ToTable("DailyActivityLogs");
            mb.Entity<DailyActivityLog>().Property(d => d.UserId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_DailyLog_User_Date", 1) { IsUnique = true } }));
            mb.Entity<DailyActivityLog>().Property(d => d.ActivityDate)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("IX_DailyLog_User_Date", 2) { IsUnique = true } }));
            mb.Entity<DailyActivityLog>().HasRequired(d => d.User).WithMany(u => u.DailyActivityLogs).HasForeignKey(d => d.UserId).WillCascadeOnDelete(true);

            // NOTIFICATION
            mb.Entity<Notification>().HasKey(n => n.NotificationId).ToTable("Notifications");
            mb.Entity<Notification>().Property(n => n.Title).HasMaxLength(200).IsRequired();
            mb.Entity<Notification>().Property(n => n.Message).HasMaxLength(1000).IsRequired();
            mb.Entity<Notification>().HasRequired(n => n.User).WithMany().HasForeignKey(n => n.UserId).WillCascadeOnDelete(true);
        }
    }
}
