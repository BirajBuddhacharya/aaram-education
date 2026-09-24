# Aaram Education

**A Comfortable, Stress-Free Learning Portal**

ASP.NET Core MVC web application for online education. Serves students, tutors, and administrators with course management, video lessons, quizzes, progress tracking, gamification, and tutor session booking.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | ASP.NET Core MVC + Razor Views (`.cshtml`) |
| Styling | CSS3 external stylesheet + Bootstrap 5 |
| Interactivity | Vanilla JavaScript |
| Backend | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 |
| Auth | ASP.NET Core Identity |
| Database | SQL Server (dev: SQLite) |
| File Storage | `wwwroot/uploads/` (phase 1) |

---

## Repository Structure

```
AaramEducation/
├── AaramEducation.sln
│
├── src/
│   ├── AaramEducation.Web/                     # MVC Web project (entry point)
│   │   ├── Controllers/
│   │   │   ├── HomeController.cs               # Landing, About, free samples
│   │   │   ├── AccountController.cs            # Register, Login, Profile
│   │   │   ├── CoursesController.cs            # Browse, enroll, view catalog
│   │   │   ├── LessonsController.cs            # Watch video, download notes
│   │   │   ├── QuizController.cs               # Take quiz, view results
│   │   │   ├── ProgressController.cs           # Student dashboard
│   │   │   ├── TutorController.cs              # Content upload, quiz creation
│   │   │   ├── AdminController.cs              # CRUD admin panel
│   │   │   ├── GuestbookController.cs          # Submit + moderate entries
│   │   │   └── SessionController.cs            # Tutor session booking
│   │   │
│   │   ├── Views/
│   │   │   ├── Shared/
│   │   │   │   ├── _Layout.cshtml              # Master layout
│   │   │   │   ├── _NavBar.cshtml
│   │   │   │   ├── _Footer.cshtml
│   │   │   │   └── Error.cshtml
│   │   │   ├── Home/
│   │   │   │   ├── Index.cshtml                # Hero + featured courses
│   │   │   │   └── About.cshtml
│   │   │   ├── Account/
│   │   │   │   ├── Register.cshtml
│   │   │   │   ├── Login.cshtml
│   │   │   │   └── Profile.cshtml
│   │   │   ├── Courses/
│   │   │   │   ├── Index.cshtml                # Course catalog
│   │   │   │   ├── Details.cshtml              # Course overview + enroll CTA
│   │   │   │   └── MyEnrollments.cshtml
│   │   │   ├── Lessons/
│   │   │   │   ├── Watch.cshtml                # Video player + notes sidebar
│   │   │   │   └── Complete.cshtml             # Lesson complete + next prompt
│   │   │   ├── Quiz/
│   │   │   │   ├── Take.cshtml                 # Quiz UI (JS-driven)
│   │   │   │   └── Results.cshtml              # Score + answer review
│   │   │   ├── Progress/
│   │   │   │   └── Dashboard.cshtml            # Student home base
│   │   │   ├── Tutor/
│   │   │   │   ├── Dashboard.cshtml
│   │   │   │   ├── ManageCourse.cshtml
│   │   │   │   ├── UploadLesson.cshtml
│   │   │   │   ├── CreateQuiz.cshtml
│   │   │   │   └── StudentProgress.cshtml
│   │   │   ├── Admin/
│   │   │   │   ├── Dashboard.cshtml
│   │   │   │   ├── Users.cshtml
│   │   │   │   ├── Courses.cshtml
│   │   │   │   ├── Guestbook.cshtml
│   │   │   │   └── Analytics.cshtml
│   │   │   ├── Guestbook/
│   │   │   │   ├── Index.cshtml                # Public guestbook view
│   │   │   │   └── Submit.cshtml
│   │   │   └── Session/
│   │   │       ├── Book.cshtml
│   │   │       └── MyBookings.cshtml
│   │   │
│   │   ├── Models/ViewModels/                  # Page-specific input/display models
│   │   │   ├── Account/
│   │   │   ├── Courses/
│   │   │   ├── Quiz/
│   │   │   ├── Tutor/
│   │   │   └── Admin/
│   │   │
│   │   ├── wwwroot/
│   │   │   ├── css/
│   │   │   │   └── aaram.css                   # Primary stylesheet
│   │   │   ├── js/
│   │   │   │   ├── quiz.js                     # Quiz logic, scoring
│   │   │   │   ├── progress.js                 # Progress bar animation
│   │   │   │   └── validation.js               # Client-side form validation
│   │   │   └── uploads/
│   │   │       ├── videos/
│   │   │       └── notes/
│   │   │
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── AaramEducation.Web.csproj
│   │
│   ├── AaramEducation.Core/                    # Domain layer — no EF/infra deps
│   │   ├── Entities/                           # Maps 1:1 to DB tables
│   │   │   ├── User.cs
│   │   │   ├── Course.cs
│   │   │   ├── Module.cs
│   │   │   ├── Lesson.cs
│   │   │   ├── Video.cs
│   │   │   ├── StudyNote.cs
│   │   │   ├── Enrollment.cs
│   │   │   ├── LessonProgress.cs
│   │   │   ├── ModuleProgress.cs
│   │   │   ├── CourseProgress.cs
│   │   │   ├── Quiz.cs
│   │   │   ├── QuizQuestion.cs
│   │   │   ├── AnswerOption.cs
│   │   │   ├── QuizAttempt.cs
│   │   │   ├── QuestionResponse.cs
│   │   │   ├── GuestbookEntry.cs
│   │   │   ├── Badge.cs
│   │   │   ├── UserBadge.cs
│   │   │   └── DailyActivityLog.cs
│   │   ├── Enums/
│   │   │   ├── UserRole.cs                     # Student | Tutor | Admin
│   │   │   ├── EnrollmentStatus.cs
│   │   │   ├── ProgressStatus.cs
│   │   │   ├── AttemptStatus.cs
│   │   │   └── ModerationStatus.cs
│   │   └── Interfaces/
│   │       ├── ICourseRepository.cs
│   │       ├── IUserRepository.cs
│   │       ├── IQuizRepository.cs
│   │       ├── IProgressRepository.cs
│   │       └── IGuestbookRepository.cs
│   │
│   └── AaramEducation.Infrastructure/          # EF Core + repos
│       ├── Data/
│       │   ├── ApplicationDbContext.cs
│       │   ├── Seed/
│       │   │   └── DbSeeder.cs
│       │   └── Migrations/
│       ├── Repositories/
│       │   ├── CourseRepository.cs
│       │   ├── UserRepository.cs
│       │   ├── QuizRepository.cs
│       │   ├── ProgressRepository.cs
│       │   └── GuestbookRepository.cs
│       └── AaramEducation.Infrastructure.csproj
│
├── tests/
│   └── AaramEducation.Tests/
│       ├── Controllers/
│       └── Repositories/
│
├── docs/
│   ├── architecture.md         # System design and layer responsibilities
│   ├── database-design.md      # Full entity definitions with fields
│   ├── modules.md              # Module-wise entity mapping + CRUD matrix
│   ├── frontend-design.md      # UI correlation with design docs
│   └── setup.md                # Dev environment setup
│
├── ERD.md                      # Mermaid ER diagram (source of truth)
└── README.md
```

---

## Modules at a Glance

| Module | Roles | Core Entities |
|---|---|---|
| Auth | All | `USER` |
| Course Catalog | Guest, Student, Tutor, Admin | `COURSE`, `MODULE` |
| Lessons | Student, Tutor | `LESSON`, `VIDEO`, `STUDY_NOTE` |
| Quiz | Student, Tutor | `QUIZ`, `QUIZ_QUESTION`, `ANSWER_OPTION`, `QUIZ_ATTEMPT`, `QUESTION_RESPONSE` |
| Progress | Student | `LESSON_PROGRESS`, `MODULE_PROGRESS`, `COURSE_PROGRESS` |
| Gamification | Student | `BADGE`, `USER_BADGE`, `DAILY_ACTIVITY_LOG` |
| Tutor Dashboard | Tutor | `COURSE`, `LESSON`, `VIDEO` |
| Admin Panel | Admin | All entities |
| Guestbook | Guest, Student, Admin | `GUESTBOOK_ENTRY` |

---

## Quick Start

See [`docs/setup.md`](docs/setup.md) for full instructions.

```bash
dotnet restore
dotnet build

# Apply migrations
dotnet ef database update \
  --project src/AaramEducation.Infrastructure \
  --startup-project src/AaramEducation.Web

dotnet run --project src/AaramEducation.Web
```

Default dev URL: `https://localhost:5001`

---

## Docs

- [Architecture](docs/architecture.md)
- [Database Design](docs/database-design.md)
- [Modules](docs/modules.md)
- [Frontend Design](docs/frontend-design.md)
- [Setup Guide](docs/setup.md)
- [ERD (Mermaid)](ERD.md)


### Test Users
┌─────────┬────────────────────┬──────────────┐
│  Role   │       Email        │   Password   │
├─────────┼────────────────────┼──────────────┤
│ Admin   │ admin@aaram.edu    │ Admin@1234   │
├─────────┼────────────────────┼──────────────┤
│ Tutor   │ tutor1@aaram.edu   │ Tutor@1234   │
├─────────┼────────────────────┼──────────────┤
│ Tutor   │ tutor2@aaram.edu   │ Tutor@1234   │
├─────────┼────────────────────┼──────────────┤
│ Student │ student1@aaram.edu │ Student@1234 │
├─────────┼────────────────────┼──────────────┤
│ Student │ student2@aaram.edu │ Student@1234 │
├─────────┼────────────────────┼──────────────┤
│ Student │ student3@aaram.edu │ Student@1234 │
└─────────┴────────────────────┴──────────────┘