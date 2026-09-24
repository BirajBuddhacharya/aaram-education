using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        // ── USERS ─────────────────────────────────────────────────────────
        var now = DateTime.UtcNow;

        var admin = new User
        {
            Email = "admin@aaram.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234"),
            FirstName = "Admin",
            LastName = "User",
            Role = UserRole.Admin,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var tutor1 = new User
        {
            Email = "tutor1@aaram.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tutor@1234"),
            FirstName = "Priya",
            LastName = "Sharma",
            Role = UserRole.Tutor,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var tutor2 = new User
        {
            Email = "tutor2@aaram.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tutor@1234"),
            FirstName = "Rajan",
            LastName = "Mehta",
            Role = UserRole.Tutor,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var student1 = new User
        {
            Email = "student1@aaram.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@1234"),
            FirstName = "Sameer",
            LastName = "Chaudhary",
            Role = UserRole.Student,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var student2 = new User
        {
            Email = "student2@aaram.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@1234"),
            FirstName = "Biraj",
            LastName = "Budhacharya",
            Role = UserRole.Student,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var student3 = new User
        {
            Email = "student3@aaram.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@1234"),
            FirstName = "Suprava",
            LastName = "Maharjan",
            Role = UserRole.Student,
            CreatedAt = now,
            UpdatedAt = now,
        };

        db.Users.AddRange(admin, tutor1, tutor2, student1, student2, student3);
        await db.SaveChangesAsync();

        // ── BADGES ────────────────────────────────────────────────────────
        var badges = new List<Badge>
        {
            new() { BadgeName = "First Step",      Description = "Complete your first lesson",         IconUrl = "/img/badges/first-step.svg",      XpReward = 10,  TargetType = "lessons_completed", TargetValue = 1  },
            new() { BadgeName = "On a Roll",        Description = "Complete 10 lessons",                IconUrl = "/img/badges/on-a-roll.svg",        XpReward = 25,  TargetType = "lessons_completed", TargetValue = 10 },
            new() { BadgeName = "Quiz Ace",         Description = "Pass 5 quizzes",                    IconUrl = "/img/badges/quiz-ace.svg",          XpReward = 30,  TargetType = "quizzes_passed",    TargetValue = 5  },
            new() { BadgeName = "Week Warrior",     Description = "Maintain a 7-day learning streak",  IconUrl = "/img/badges/week-warrior.svg",      XpReward = 50,  TargetType = "streak_days",       TargetValue = 7  },
            new() { BadgeName = "Course Finisher",  Description = "Complete an entire course",         IconUrl = "/img/badges/course-finisher.svg",   XpReward = 100, TargetType = "courses_completed", TargetValue = 1  },
        };
        db.Badges.AddRange(badges);
        await db.SaveChangesAsync();

        // ── COURSE 1: GCSE Mathematics ────────────────────────────────────
        var mathsCourse = new Course
        {
            CreatedByUserId = tutor1.UserId,
            CourseName = "GCSE Mathematics",
            CourseDescription = "A structured course covering core GCSE Maths topics including algebra, geometry, and statistics. Designed for Year 10–11 students preparing for their exams.",
            Subject = "Mathematics",
            DifficultyLevel = "Intermediate",
            IsPublished = true,
            CreatedAt = now,
        };

        var computingCourse = new Course
        {
            CreatedByUserId = tutor2.UserId,
            CourseName = "Introduction to Computing",
            CourseDescription = "Beginner-friendly introduction to programming and computing concepts. No prior experience required — start from scratch and build confidence with code.",
            Subject = "Computing",
            DifficultyLevel = "Beginner",
            IsPublished = true,
            CreatedAt = now,
        };

        db.Courses.AddRange(mathsCourse, computingCourse);
        await db.SaveChangesAsync();

        // ── MODULES & LESSONS: GCSE Mathematics ───────────────────────────
        var mathsModule1 = new Module { CourseId = mathsCourse.CourseId, ModuleName = "Numbers & Algebra",     SequenceOrder = 1, CreatedAt = now };
        var mathsModule2 = new Module { CourseId = mathsCourse.CourseId, ModuleName = "Geometry & Measurement", SequenceOrder = 2, CreatedAt = now };
        db.Modules.AddRange(mathsModule1, mathsModule2);
        await db.SaveChangesAsync();

        var mathsLessons = new List<Lesson>
        {
            new() { ModuleId = mathsModule1.ModuleId, LessonTitle = "Introduction to Algebra",    SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
            new() { ModuleId = mathsModule1.ModuleId, LessonTitle = "Solving Linear Equations",   SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
            new() { ModuleId = mathsModule1.ModuleId, LessonTitle = "Quadratic Equations",         SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
            new() { ModuleId = mathsModule2.ModuleId, LessonTitle = "Angles and Triangles",        SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
            new() { ModuleId = mathsModule2.ModuleId, LessonTitle = "Circles and Arcs",            SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
            new() { ModuleId = mathsModule2.ModuleId, LessonTitle = "Area and Volume",             SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
        };
        db.Lessons.AddRange(mathsLessons);
        await db.SaveChangesAsync();

        // ── MODULES & LESSONS: Introduction to Computing ───────────────────
        var compModule1 = new Module { CourseId = computingCourse.CourseId, ModuleName = "Fundamentals of Programming", SequenceOrder = 1, CreatedAt = now };
        var compModule2 = new Module { CourseId = computingCourse.CourseId, ModuleName = "Data Structures",              SequenceOrder = 2, CreatedAt = now };
        db.Modules.AddRange(compModule1, compModule2);
        await db.SaveChangesAsync();

        var compLessons = new List<Lesson>
        {
            new() { ModuleId = compModule1.ModuleId, LessonTitle = "What is Programming?",       SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
            new() { ModuleId = compModule1.ModuleId, LessonTitle = "Variables and Data Types",   SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
            new() { ModuleId = compModule1.ModuleId, LessonTitle = "Control Flow",               SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
            new() { ModuleId = compModule2.ModuleId, LessonTitle = "Arrays and Lists",           SequenceOrder = 1, IsFreeSample = true,  CreatedAt = now },
            new() { ModuleId = compModule2.ModuleId, LessonTitle = "Dictionaries and Sets",      SequenceOrder = 2, IsFreeSample = false, CreatedAt = now },
            new() { ModuleId = compModule2.ModuleId, LessonTitle = "Stacks and Queues",          SequenceOrder = 3, IsFreeSample = false, CreatedAt = now },
        };
        db.Lessons.AddRange(compLessons);
        await db.SaveChangesAsync();

        // ── VIDEOS + STUDY NOTES (one per lesson) ─────────────────────────
        var allLessons = mathsLessons.Concat(compLessons).ToList();
        foreach (var lesson in allLessons)
        {
            db.Videos.Add(new Video
            {
                LessonId = lesson.LessonId,
                VideoTitle = lesson.LessonTitle,
                VideoUrl = $"/uploads/videos/sample_{lesson.LessonId}.mp4",
                DurationSeconds = 480, // 8 minutes
                UploadedAt = now,
            });
            db.StudyNotes.Add(new StudyNote
            {
                LessonId = lesson.LessonId,
                NoteTitle = $"{lesson.LessonTitle} — Study Notes",
                NoteContent = $"<p>Key concepts for <strong>{lesson.LessonTitle}</strong>. Download the PDF for the full notes.</p>",
                FileUrl = $"/uploads/notes/notes_{lesson.LessonId}.pdf",
                CreatedAt = now,
            });
        }
        await db.SaveChangesAsync();

        // ── QUIZZES + QUESTIONS + OPTIONS ─────────────────────────────────
        var quizData = new Dictionary<int, (string Title, List<(string Q, string[] Opts, int Correct)> Questions)>
        {
            [mathsLessons[0].LessonId] = ("Algebra Basics Quiz", new()
            {
                ("What does a variable represent in algebra?",       ["A fixed number", "An unknown value", "A formula", "An equation"], 1),
                ("Which of the following is an algebraic expression?", ["12 + 5", "3x + 7", "100%", "π"], 1),
                ("What is the value of x if x + 4 = 9?",            ["3", "4", "5", "13"], 2),
                ("Which symbol means 'not equal to'?",               ["=", "≈", "≠", "≡"], 2),
                ("Simplify: 2x + 3x",                                ["5", "5x²", "5x", "6x"], 2),
            }),
            [mathsLessons[1].LessonId] = ("Linear Equations Quiz", new()
            {
                ("Solve: 2x = 10",                                   ["x = 2", "x = 5", "x = 8", "x = 20"], 1),
                ("Solve: x − 3 = 7",                                 ["x = 4", "x = 10", "x = 21", "x = −4"], 1),
                ("Which step comes first when solving 3x + 6 = 15?", ["Divide by 3", "Subtract 6 from both sides", "Add 6", "Multiply both sides by 3"], 1),
                ("Solve: x/4 = 3",                                   ["x = 7", "x = 0.75", "x = 12", "x = 1"], 2),
                ("What is a linear equation?",                        ["Has x²", "Has no variables", "Has one variable to the first power", "Has two unknowns"], 2),
            }),
            [mathsLessons[2].LessonId] = ("Quadratic Equations Quiz", new()
            {
                ("What is the standard form of a quadratic equation?", ["ax + b = 0", "ax² + bx + c = 0", "ax³ + b = 0", "a/x = b"], 1),
                ("The solutions of a quadratic are called?",           ["Roots", "Coefficients", "Constants", "Gradients"], 0),
                ("How many roots can a quadratic equation have?",      ["Always 1", "Always 2", "0, 1, or 2", "Always 3"], 2),
                ("Solve x² = 25",                                      ["x = 5", "x = −5", "x = 5 or x = −5", "x = 625"], 2),
                ("What does the discriminant b²−4ac determine?",       ["The y-intercept", "The number of real roots", "The vertex", "The axis of symmetry"], 1),
            }),
            [mathsLessons[3].LessonId] = ("Angles Quiz", new()
            {
                ("Angles in a triangle always add up to?",             ["90°", "180°", "270°", "360°"], 1),
                ("What type of angle is 90°?",                         ["Acute", "Right", "Obtuse", "Reflex"], 1),
                ("An equilateral triangle has angles of?",             ["90°, 45°, 45°", "60°, 60°, 60°", "120°, 30°, 30°", "90°, 60°, 30°"], 1),
                ("Angles on a straight line add up to?",               ["90°", "270°", "360°", "180°"], 3),
                ("What is a reflex angle?",                            ["Between 0° and 90°", "Exactly 90°", "Between 90° and 180°", "Between 180° and 360°"], 3),
            }),
            [mathsLessons[4].LessonId] = ("Circles Quiz", new()
            {
                ("What is the diameter of a circle?",                  ["Half the radius", "Twice the radius", "The circumference", "The area"], 1),
                ("Formula for circumference of a circle?",             ["πr²", "2πr", "4πr", "πr"], 1),
                ("Formula for the area of a circle?",                  ["2πr", "πd", "πr²", "πr³"], 2),
                ("An arc is?",                                         ["The full circle", "A chord", "Part of the circumference", "The centre point"], 2),
                ("π (pi) is approximately equal to?",                  ["2.14", "3.14", "1.41", "4.14"], 1),
            }),
            [mathsLessons[5].LessonId] = ("Area and Volume Quiz", new()
            {
                ("Area of a rectangle with length 5 and width 3?",    ["8", "15", "16", "30"], 1),
                ("Volume of a cube with side 4?",                     ["16", "24", "64", "12"], 2),
                ("Area is measured in?",                              ["cm", "cm²", "cm³", "m"], 1),
                ("Volume of a cuboid 2×3×4?",                        ["9", "18", "24", "36"], 2),
                ("Area of a triangle with base 6 and height 4?",     ["24", "12", "10", "48"], 1),
            }),
            [compLessons[0].LessonId] = ("Intro to Programming Quiz", new()
            {
                ("What is a program?",                                ["A type of computer", "A set of instructions for a computer", "A programming language", "An operating system"], 1),
                ("Which of these is a programming language?",         ["HTML", "Python", "HTTP", "PDF"], 1),
                ("What does 'syntax' mean in programming?",           ["The output of a program", "The speed of execution", "The rules for writing valid code", "The memory used"], 2),
                ("What is a bug in programming?",                     ["A type of variable", "An error in the code", "A programming language feature", "A hardware component"], 1),
                ("What does 'debugging' mean?",                       ["Writing new code", "Deleting old code", "Finding and fixing errors", "Running a program"], 2),
            }),
            [compLessons[1].LessonId] = ("Variables Quiz", new()
            {
                ("What is a variable?",                               ["A fixed constant", "A named storage location", "A type of loop", "A programming language"], 1),
                ("Which data type stores whole numbers?",             ["String", "Boolean", "Float", "Integer"], 3),
                ("What is the Boolean data type?",                    ["Text data", "True or False", "Decimal numbers", "Lists"], 1),
                ("Which is a valid variable name in most languages?", ["2name", "my_name", "my-name", "my name"], 1),
                ("What does 'string' mean in programming?",           ["A number", "A sequence of characters (text)", "A condition", "A loop"], 1),
            }),
            [compLessons[2].LessonId] = ("Control Flow Quiz", new()
            {
                ("What does an if-statement do?",                     ["Repeats code", "Stores a value", "Makes a decision", "Defines a function"], 2),
                ("What is a loop used for?",                          ["Making decisions", "Repeating a block of code", "Storing data", "Defining variables"], 1),
                ("How many times does a 'while True' loop run?",      ["Once", "Zero times", "Infinitely (unless broken)", "Ten times"], 2),
                ("What does 'break' do inside a loop?",               ["Continues the loop", "Exits the loop", "Skips the current iteration", "Starts the loop over"], 1),
                ("What is an else clause?",                           ["Runs when if is true", "Runs when if is false", "Always runs", "Never runs"], 1),
            }),
            [compLessons[3].LessonId] = ("Arrays and Lists Quiz", new()
            {
                ("What is an array?",                                 ["A single value", "A collection of values", "A function", "A type of loop"], 1),
                ("What is the index of the first element in most languages?", ["1", "0", "-1", "2"], 1),
                ("How do you access the third element of list myList?", ["myList[3]", "myList[2]", "myList(3)", "myList{2}"], 1),
                ("What is the length of [1, 2, 3, 4, 5]?",           ["4", "6", "5", "3"], 2),
                ("What does append() do to a list?",                  ["Removes an item", "Adds an item to the end", "Sorts the list", "Clears the list"], 1),
            }),
            [compLessons[4].LessonId] = ("Dictionaries Quiz", new()
            {
                ("A dictionary stores data as?",                      ["Ordered lists", "Key-value pairs", "Tuples", "Single values"], 1),
                ("How do you access a value in a dict?",              ["dict[0]", "dict.get()", "dict['key']", "dict.value()"], 2),
                ("Are dictionary keys unique?",                        ["No", "Sometimes", "Yes", "Only for integers"], 2),
                ("What is a set?",                                    ["An ordered list", "A collection of unique elements", "A key-value store", "A tuple"], 1),
                ("Which removes duplicates from a list? lst = [1,1,2]", ["list(sorted(lst))", "list(set(lst))", "list(dict(lst))", "list(tuple(lst))"], 1),
            }),
            [compLessons[5].LessonId] = ("Stacks and Queues Quiz", new()
            {
                ("A stack follows which principle?",                   ["FIFO", "LIFO", "FILO", "LILO"], 1),
                ("A queue follows which principle?",                   ["LIFO", "FILO", "FIFO", "LILO"], 2),
                ("In a stack, push() does what?",                     ["Removes top item", "Adds item to top", "Clears the stack", "Sorts the stack"], 1),
                ("In a queue, dequeue() does what?",                  ["Adds to the back", "Removes from the front", "Removes from the back", "Clears the queue"], 1),
                ("Which real-world example is a queue?",              ["Browser back button", "Undo in a text editor", "Printer job list", "Call stack"], 2),
            }),
        };

        foreach (var (lessonId, (title, questions)) in quizData)
        {
            var quiz = new Quiz
            {
                LessonId = lessonId,
                QuizTitle = title,
                PassingScore = 60,
                MaxAttempts = 3,
                CreatedAt = now,
            };
            db.Quizzes.Add(quiz);
            await db.SaveChangesAsync();

            for (int qi = 0; qi < questions.Count; qi++)
            {
                var (qText, opts, correctIdx) = questions[qi];
                var question = new QuizQuestion
                {
                    QuizId = quiz.QuizId,
                    QuestionText = qText,
                    QuestionType = QuestionType.MultipleChoice,
                    SequenceOrder = qi + 1,
                    PointsValue = 1,
                };
                db.QuizQuestions.Add(question);
                await db.SaveChangesAsync();

                for (int oi = 0; oi < opts.Length; oi++)
                {
                    db.AnswerOptions.Add(new AnswerOption
                    {
                        QuestionId = question.QuestionId,
                        OptionText = opts[oi],
                        IsCorrect = oi == correctIdx,
                        SequenceOrder = oi + 1,
                    });
                }
                await db.SaveChangesAsync();
            }
        }

        // ── ENROLLMENTS ───────────────────────────────────────────────────
        var enrollment1 = new Enrollment
        {
            StudentId = student1.UserId,
            CourseId = mathsCourse.CourseId,
            EnrollmentDate = now.AddDays(-14),
            EnrollmentStatus = EnrollmentStatus.Active,
        };
        var enrollment2 = new Enrollment
        {
            StudentId = student2.UserId,
            CourseId = mathsCourse.CourseId,
            EnrollmentDate = now.AddDays(-7),
            EnrollmentStatus = EnrollmentStatus.Active,
        };
        var enrollment3 = new Enrollment
        {
            StudentId = student3.UserId,
            CourseId = computingCourse.CourseId,
            EnrollmentDate = now.AddDays(-5),
            EnrollmentStatus = EnrollmentStatus.Active,
        };
        db.Enrollments.AddRange(enrollment1, enrollment2, enrollment3);
        await db.SaveChangesAsync();

        db.CourseProgresses.AddRange(
            new CourseProgress { EnrollmentId = enrollment1.EnrollmentId, Status = ProgressStatus.InProgress, LessonsCompleted = 2, ModulesCompleted = 0, PercentComplete = 33.3f, LastAccessedAt = now.AddDays(-1) },
            new CourseProgress { EnrollmentId = enrollment2.EnrollmentId, Status = ProgressStatus.NotStarted, LessonsCompleted = 0, ModulesCompleted = 0, PercentComplete = 0f,    LastAccessedAt = now.AddDays(-7) },
            new CourseProgress { EnrollmentId = enrollment3.EnrollmentId, Status = ProgressStatus.InProgress, LessonsCompleted = 1, ModulesCompleted = 0, PercentComplete = 16.7f, LastAccessedAt = now }
        );
        await db.SaveChangesAsync();

        // ── GUESTBOOK ENTRIES ─────────────────────────────────────────────
        db.GuestbookEntries.AddRange(
            new GuestbookEntry
            {
                GuestName = "Ayus Yadav",
                GuestEmail = "ayus@example.com",
                Message = "Aaram Education made revision feel so much less overwhelming. The short lessons fit perfectly into my lunch breaks!",
                SubmittedAt = now.AddDays(-10),
                ModerationStatus = ModerationStatus.Approved,
                ModeratedBy = admin.UserId,
                ModeratedAt = now.AddDays(-9),
            },
            new GuestbookEntry
            {
                GuestName = "Anita Rai",
                GuestEmail = null,
                Message = "I love that I can pick up where I left off. The quizzes help me know exactly which topics need more practice.",
                SubmittedAt = now.AddDays(-5),
                ModerationStatus = ModerationStatus.Approved,
                ModeratedBy = admin.UserId,
                ModeratedAt = now.AddDays(-4),
            },
            new GuestbookEntry
            {
                GuestName = "Marcus Thompson",
                GuestEmail = "marcus@example.com",
                Message = "As a working professional, I appreciate the flexibility. Finally a platform that respects my time.",
                SubmittedAt = now.AddDays(-2),
                ModerationStatus = ModerationStatus.Approved,
                ModeratedBy = admin.UserId,
                ModeratedAt = now.AddDays(-1),
            },
            new GuestbookEntry
            {
                GuestName = "Spam User",
                GuestEmail = "spam@spam.com",
                Message = "Buy cheap products at spam-site.com!!!",
                SubmittedAt = now.AddHours(-3),
                ModerationStatus = ModerationStatus.Rejected,
                ModeratedBy = admin.UserId,
                ModeratedAt = now.AddHours(-2),
            },
            new GuestbookEntry
            {
                GuestName = "Pending User",
                GuestEmail = null,
                Message = "Just found this site. Looks promising, will update after trying a few lessons.",
                SubmittedAt = now.AddHours(-1),
                ModerationStatus = ModerationStatus.Pending,
            }
        );
        await db.SaveChangesAsync();
    }
}
