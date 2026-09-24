using System;
using System.Collections.Generic;
using System.Linq;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;

namespace AaramEducation.Infrastructure.Data.Seed
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (db.Users.Any()) return;

            var now = DateTime.UtcNow;

            var admin = new User { Email = "admin@aaram.edu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234"), FirstName = "Admin", LastName = "User", Role = UserRole.Admin, CreatedAt = now, UpdatedAt = now };
            var tutor1 = new User { Email = "tutor1@aaram.edu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tutor@1234"), FirstName = "Priya", LastName = "Sharma", Role = UserRole.Tutor, CreatedAt = now, UpdatedAt = now };
            var tutor2 = new User { Email = "tutor2@aaram.edu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tutor@1234"), FirstName = "Rajan", LastName = "Mehta", Role = UserRole.Tutor, CreatedAt = now, UpdatedAt = now };
            var student1 = new User { Email = "student1@aaram.edu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@1234"), FirstName = "Sameer", LastName = "Chaudhary", Role = UserRole.Student, CreatedAt = now, UpdatedAt = now };
            var student2 = new User { Email = "student2@aaram.edu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@1234"), FirstName = "Biraj", LastName = "Budhacharya", Role = UserRole.Student, CreatedAt = now, UpdatedAt = now };
            var student3 = new User { Email = "student3@aaram.edu", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@1234"), FirstName = "Suprava", LastName = "Maharjan", Role = UserRole.Student, CreatedAt = now, UpdatedAt = now };

            db.Users.AddRange(admin, tutor1, tutor2, student1, student2, student3);
            db.SaveChanges();

            var badges = new List<Badge>
            {
                new Badge { BadgeName = "First Step",     Description = "Complete your first lesson",        IconUrl = "/img/badges/first-step.svg",     XpReward = 10,  TargetType = "lessons_completed", TargetValue = 1  },
                new Badge { BadgeName = "On a Roll",       Description = "Complete 10 lessons",               IconUrl = "/img/badges/on-a-roll.svg",       XpReward = 25,  TargetType = "lessons_completed", TargetValue = 10 },
                new Badge { BadgeName = "Quiz Ace",        Description = "Pass 5 quizzes",                   IconUrl = "/img/badges/quiz-ace.svg",         XpReward = 30,  TargetType = "quizzes_passed",    TargetValue = 5  },
                new Badge { BadgeName = "Week Warrior",    Description = "Maintain a 7-day learning streak", IconUrl = "/img/badges/week-warrior.svg",     XpReward = 50,  TargetType = "streak_days",       TargetValue = 7  },
                new Badge { BadgeName = "Course Finisher", Description = "Complete an entire course",        IconUrl = "/img/badges/course-finisher.svg",  XpReward = 100, TargetType = "courses_completed", TargetValue = 1  },
            };
            db.Badges.AddRange(badges);
            db.SaveChanges();

            var mathsCourse = new Course { CreatedByUserId = tutor1.UserId, CourseName = "GCSE Mathematics", CourseDescription = "A structured course covering core GCSE Maths topics.", Subject = "Mathematics", DifficultyLevel = "Intermediate", IsPublished = true, CreatedAt = now };
            var computingCourse = new Course { CreatedByUserId = tutor2.UserId, CourseName = "Introduction to Computing", CourseDescription = "Beginner-friendly introduction to programming.", Subject = "Computing", DifficultyLevel = "Beginner", IsPublished = true, CreatedAt = now };
            db.Courses.AddRange(mathsCourse, computingCourse);
            db.SaveChanges();

            var mathsModule1 = new Module { CourseId = mathsCourse.CourseId, ModuleName = "Numbers & Algebra",      SequenceOrder = 1, CreatedAt = now };
            var mathsModule2 = new Module { CourseId = mathsCourse.CourseId, ModuleName = "Geometry & Measurement", SequenceOrder = 2, CreatedAt = now };
            db.Modules.AddRange(mathsModule1, mathsModule2);
            db.SaveChanges();

            var mathsLessons = new List<Lesson>
            {
                new Lesson { ModuleId = mathsModule1.ModuleId, LessonTitle = "Introduction to Algebra",  SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
                new Lesson { ModuleId = mathsModule1.ModuleId, LessonTitle = "Solving Linear Equations", SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
                new Lesson { ModuleId = mathsModule1.ModuleId, LessonTitle = "Quadratic Equations",      SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
                new Lesson { ModuleId = mathsModule2.ModuleId, LessonTitle = "Angles and Triangles",     SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
                new Lesson { ModuleId = mathsModule2.ModuleId, LessonTitle = "Circles and Arcs",         SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
                new Lesson { ModuleId = mathsModule2.ModuleId, LessonTitle = "Area and Volume",          SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
            };
            db.Lessons.AddRange(mathsLessons);
            db.SaveChanges();

            var compModule1 = new Module { CourseId = computingCourse.CourseId, ModuleName = "Fundamentals of Programming", SequenceOrder = 1, CreatedAt = now };
            var compModule2 = new Module { CourseId = computingCourse.CourseId, ModuleName = "Data Structures",             SequenceOrder = 2, CreatedAt = now };
            db.Modules.AddRange(compModule1, compModule2);
            db.SaveChanges();

            var compLessons = new List<Lesson>
            {
                new Lesson { ModuleId = compModule1.ModuleId, LessonTitle = "What is Programming?",     SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
                new Lesson { ModuleId = compModule1.ModuleId, LessonTitle = "Variables and Data Types", SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
                new Lesson { ModuleId = compModule1.ModuleId, LessonTitle = "Control Flow",             SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
                new Lesson { ModuleId = compModule2.ModuleId, LessonTitle = "Arrays and Lists",         SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
                new Lesson { ModuleId = compModule2.ModuleId, LessonTitle = "Dictionaries and Sets",    SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
                new Lesson { ModuleId = compModule2.ModuleId, LessonTitle = "Stacks and Queues",        SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
            };
            db.Lessons.AddRange(compLessons);
            db.SaveChanges();

            var allLessons = mathsLessons.Concat(compLessons).ToList();
            foreach (var lesson in allLessons)
            {
                db.Videos.Add(new Video { LessonId = lesson.LessonId, VideoTitle = lesson.LessonTitle, VideoUrl = $"/uploads/videos/sample_{lesson.LessonId}.mp4", DurationSeconds = 480, UploadedAt = now });
                db.StudyNotes.Add(new StudyNote { LessonId = lesson.LessonId, NoteTitle = $"{lesson.LessonTitle} — Study Notes", NoteContent = $"<p>Key concepts for <strong>{lesson.LessonTitle}</strong>.</p>", FileUrl = $"/uploads/notes/notes_{lesson.LessonId}.pdf", CreatedAt = now });
            }
            db.SaveChanges();

            // Sample quiz for first lesson
            var quiz = new Quiz { LessonId = mathsLessons[0].LessonId, QuizTitle = "Algebra Basics Quiz", PassingScore = 60, MaxAttempts = 3, CreatedAt = now };
            db.Quizzes.Add(quiz);
            db.SaveChanges();

            var q1 = new QuizQuestion { QuizId = quiz.QuizId, QuestionText = "What does a variable represent in algebra?", QuestionType = QuestionType.MultipleChoice, SequenceOrder = 1, PointsValue = 1 };
            db.QuizQuestions.Add(q1);
            db.SaveChanges();
            db.AnswerOptions.AddRange(
                new AnswerOption { QuestionId = q1.QuestionId, OptionText = "A fixed number",    IsCorrect = false, SequenceOrder = 1 },
                new AnswerOption { QuestionId = q1.QuestionId, OptionText = "An unknown value",  IsCorrect = true,  SequenceOrder = 2 },
                new AnswerOption { QuestionId = q1.QuestionId, OptionText = "A formula",         IsCorrect = false, SequenceOrder = 3 },
                new AnswerOption { QuestionId = q1.QuestionId, OptionText = "An equation",       IsCorrect = false, SequenceOrder = 4 }
            );
            db.SaveChanges();

            var enrollment1 = new Enrollment { StudentId = student1.UserId, CourseId = mathsCourse.CourseId, EnrollmentDate = now.AddDays(-14), EnrollmentStatus = EnrollmentStatus.Active };
            var enrollment2 = new Enrollment { StudentId = student2.UserId, CourseId = mathsCourse.CourseId, EnrollmentDate = now.AddDays(-7),  EnrollmentStatus = EnrollmentStatus.Active };
            var enrollment3 = new Enrollment { StudentId = student3.UserId, CourseId = computingCourse.CourseId, EnrollmentDate = now.AddDays(-5), EnrollmentStatus = EnrollmentStatus.Active };
            db.Enrollments.AddRange(enrollment1, enrollment2, enrollment3);
            db.SaveChanges();

            db.CourseProgresses.AddRange(
                new CourseProgress { EnrollmentId = enrollment1.EnrollmentId, Status = ProgressStatus.InProgress, LessonsCompleted = 2, PercentComplete = 33.3f, LastAccessedAt = now.AddDays(-1) },
                new CourseProgress { EnrollmentId = enrollment2.EnrollmentId, Status = ProgressStatus.NotStarted, LessonsCompleted = 0, PercentComplete = 0f,    LastAccessedAt = now.AddDays(-7) },
                new CourseProgress { EnrollmentId = enrollment3.EnrollmentId, Status = ProgressStatus.InProgress, LessonsCompleted = 1, PercentComplete = 16.7f, LastAccessedAt = now }
            );
            db.SaveChanges();

            db.GuestbookEntries.AddRange(
                new GuestbookEntry { GuestName = "Ayus Yadav",      GuestEmail = "ayus@example.com",   Message = "Aaram Education made revision feel less overwhelming!", SubmittedAt = now.AddDays(-10), ModerationStatus = ModerationStatus.Approved, ModeratedBy = admin.UserId, ModeratedAt = now.AddDays(-9) },
                new GuestbookEntry { GuestName = "Anita Rai",       GuestEmail = null,                 Message = "I love that I can pick up where I left off.",           SubmittedAt = now.AddDays(-5),  ModerationStatus = ModerationStatus.Approved, ModeratedBy = admin.UserId, ModeratedAt = now.AddDays(-4) },
                new GuestbookEntry { GuestName = "Marcus Thompson",  GuestEmail = "marcus@example.com", Message = "Finally a platform that respects my time.",             SubmittedAt = now.AddDays(-2),  ModerationStatus = ModerationStatus.Approved, ModeratedBy = admin.UserId, ModeratedAt = now.AddDays(-1) },
                new GuestbookEntry { GuestName = "Pending User",     GuestEmail = null,                 Message = "Just found this site. Looks promising!",                SubmittedAt = now.AddHours(-1), ModerationStatus = ModerationStatus.Pending }
            );
            db.SaveChanges();
        }
    }
}
