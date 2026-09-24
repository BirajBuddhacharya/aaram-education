# Implementation Plan

Grouped by phase. Each phase must be complete before the next begins.  
Work top-to-bottom within each phase — tasks have dependencies.

---

## Phase 1 — Project Scaffold

**Goal:** Running solution with empty shell, DB connected, Identity wired.

- [ ] 1.1 Create solution and three projects
  ```bash
  dotnet new sln -n AaramEducation
  dotnet new mvc -n AaramEducation.Web -o src/AaramEducation.Web
  dotnet new classlib -n AaramEducation.Core -o src/AaramEducation.Core
  dotnet new classlib -n AaramEducation.Infrastructure -o src/AaramEducation.Infrastructure
  dotnet sln add src/AaramEducation.Web src/AaramEducation.Core src/AaramEducation.Infrastructure
  ```

- [ ] 1.2 Add project references
  - Web → Core, Infrastructure
  - Infrastructure → Core

- [ ] 1.3 Install NuGet packages
  - `AaramEducation.Infrastructure`: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Tools`
  - `AaramEducation.Web`: `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Design`

- [ ] 1.4 Create `ApplicationDbContext` in Infrastructure
  - Inherit `IdentityDbContext<User>` (or plain `DbContext` if using custom identity)
  - Add empty `DbSet<>` stubs for all entities — fill in Fluent API after entities are created

- [ ] 1.5 Wire up `Program.cs`
  - Register `ApplicationDbContext` with SQLite (dev) connection string
  - Register ASP.NET Core Identity (`AddDefaultIdentity`, `AddRoles`)
  - Register repository interfaces → implementations (DI)

- [ ] 1.6 Add `appsettings.Development.json` with SQLite connection string

- [ ] 1.7 Initial migration and `dotnet run` — confirm home page loads at `https://localhost:5001`

---

## Phase 2 — Core Entities & Database

**Goal:** All 19 ERD tables created via EF Core migrations.

- [ ] 2.1 Create enums in `AaramEducation.Core/Enums/`
  - `UserRole.cs` — `Student | Tutor | Admin`
  - `EnrollmentStatus.cs` — `Active | Completed | Dropped`
  - `ProgressStatus.cs` — `NotStarted | InProgress | Completed`
  - `AttemptStatus.cs` — `InProgress | Submitted | Graded`
  - `ModerationStatus.cs` — `Pending | Approved | Rejected`
  - `QuestionType.cs` — `MultipleChoice | TrueFalse | ShortAnswer`

- [ ] 2.2 Create entity classes in `AaramEducation.Core/Entities/`  
  Order matters — create referenced entities first:
  1. `User.cs` (extends IdentityUser or standalone)
  2. `Course.cs`
  3. `Module.cs` (FK → Course)
  4. `Lesson.cs` (FK → Module)
  5. `Video.cs` (FK → Lesson)
  6. `StudyNote.cs` (FK → Lesson)
  7. `Enrollment.cs` (FK → User, Course)
  8. `CourseProgress.cs` (FK → Enrollment, 1-to-1)
  9. `ModuleProgress.cs` (FK → User, Module)
  10. `LessonProgress.cs` (FK → User, Lesson)
  11. `Quiz.cs` (FK → Lesson)
  12. `QuizQuestion.cs` (FK → Quiz)
  13. `AnswerOption.cs` (FK → QuizQuestion)
  14. `QuizAttempt.cs` (FK → User, Quiz)
  15. `QuestionResponse.cs` (FK → QuizAttempt, QuizQuestion, AnswerOption nullable)
  16. `GuestbookEntry.cs` (FK → User nullable)
  17. `Badge.cs`
  18. `UserBadge.cs` (FK → User, Badge)
  19. `DailyActivityLog.cs` (FK → User)

- [ ] 2.3 Configure Fluent API in `ApplicationDbContext.OnModelCreating`
  - Unique constraints: `(studentID, courseID)` on Enrollment; `(studentID, lessonID)` on LessonProgress; `(studentID, moduleID)` on ModuleProgress; `(userID, activityDate)` on DailyActivityLog; `(userID, badgeID)` on UserBadge; `enrollmentID` UK on CourseProgress
  - Cascade delete rules (e.g. delete Course → cascade delete Modules, Lessons, Videos, Notes, Quizzes)
  - String length limits matching database-design.md

- [ ] 2.4 Add `DbSet<>` for all entities in `ApplicationDbContext`

- [ ] 2.5 Generate and apply migration
  ```bash
  dotnet ef migrations add InitialSchema \
    --project src/AaramEducation.Infrastructure \
    --startup-project src/AaramEducation.Web
  dotnet ef database update \
    --project src/AaramEducation.Infrastructure \
    --startup-project src/AaramEducation.Web
  ```

- [ ] 2.6 Write `DbSeeder.cs` — seed on app start in Development:
  - 1 Admin, 2 Tutors, 3 Students
  - 2 published Courses with 2 Modules each, 3 Lessons per Module
  - 1 Video and 1 StudyNote per Lesson
  - 1 Quiz with 5 Questions (MCQ) per Lesson
  - Badge definitions (5 badges)
  - 3 approved GuestbookEntry rows

---

## Phase 3 — Repository Layer

**Goal:** Controller code never touches `DbContext` directly.

- [ ] 3.1 Define interfaces in `AaramEducation.Core/Interfaces/`
  - `IUserRepository` — GetById, GetByEmail, Update
  - `ICourseRepository` — GetAll (published), GetById (with modules/lessons), GetByTutor, Create, Update, Delete, Publish
  - `IEnrollmentRepository` — GetByStudent, GetByCourse, Enroll, Drop, IsEnrolled
  - `ILessonRepository` — GetById (with video/notes/quiz), GetByCourse
  - `IProgressRepository` — GetLessonProgress, UpsertLessonProgress, GetCourseProgress, UpdateRollups, UpsertDailyLog
  - `IQuizRepository` — GetWithQuestions, GetAttemptsByStudent, SaveAttempt, SaveResponses, GradeAttempt
  - `IGuestbookRepository` — GetApproved, GetPending, Submit, Moderate
  - `IBadgeRepository` — GetAll, CheckAndAward

- [ ] 3.2 Implement each interface in `AaramEducation.Infrastructure/Repositories/`  
  Use `.AsNoTracking()` on all read-only queries.

- [ ] 3.3 Register all repositories in `Program.cs`
  ```csharp
  builder.Services.AddScoped<ICourseRepository, CourseRepository>();
  // ... etc
  ```

---

## Phase 4 — Auth Module

**Goal:** Register, login, logout, profile — all roles working.

- [ ] 4.1 `AccountController` actions: `Register GET/POST`, `Login GET/POST`, `Logout POST`, `Profile GET/POST`

- [ ] 4.2 ViewModels in `Models/ViewModels/Account/`
  - `RegisterViewModel` — firstName, lastName, email, password, confirmPassword, role (Student/Tutor only)
  - `LoginViewModel` — email, password, rememberMe
  - `ProfileViewModel` — firstName, lastName, email, profilePicture

- [ ] 4.3 Razor Views: `Register.cshtml`, `Login.cshtml`, `Profile.cshtml`
  - Use `<form asp-action>` tag helpers
  - `<span asp-validation-for>` on every field
  - `@Html.AntiForgeryToken()` on every form

- [ ] 4.4 Client-side `validation.js`
  - Required field check before submit
  - Email regex
  - Password: min 8 chars, 1 uppercase, 1 number
  - Password confirm match

- [ ] 4.5 Profile picture upload — save to `wwwroot/uploads/avatars/{userId}.jpg`, update `User.profilePicture`

- [ ] 4.6 Test all three roles: Student, Tutor, Admin redirect to correct dashboard after login

---

## Phase 5 — Layout & Shared UI

**Goal:** `_Layout.cshtml` with navbar, footer, theme tokens — all pages inherit it.

- [ ] 5.1 Write `aaram.css` with CSS custom properties
  - Palette tokens (light + dark), typography scale, spacing scale
  - `.btn-primary`, `.btn-secondary`, `.card`, `.badge-pill`, `.progress-bar` base styles

- [ ] 5.2 `_Layout.cshtml`
  - Link Bootstrap 5 (CDN), `aaram.css`, Google Fonts
  - `@RenderBody()`, `@RenderSection("Scripts", required: false)`

- [ ] 5.3 `_NavBar.cshtml` partial
  - Logo + site name
  - Links: Courses, About, Guestbook
  - Conditional: `[Login | Register]` for guests, `[Dashboard | Profile | Logout]` for logged-in
  - Notification count badge (unread, from ViewBag or ViewComponent)

- [ ] 5.4 `_Footer.cshtml` partial

- [ ] 5.5 Breadcrumb partial — accepts `IEnumerable<(string Label, string Url)>`, renders `<nav aria-label="breadcrumb">`

---

## Phase 6 — Course & Enrollment Module

**Goal:** Students can browse, view, and enroll in courses.

- [ ] 6.1 `CoursesController`
  - `Index` — all published courses, filter by subject/difficulty (query string params)
  - `Details(int id)` — course + modules + lessons (lock icon on non-free lessons if not enrolled)
  - `Enroll(int courseId)` POST — creates Enrollment + CourseProgress row
  - `Drop(int courseId)` POST — sets status = Dropped
  - `MyEnrollments` — student's active enrollments with CourseProgress %

- [ ] 6.2 ViewModels: `CourseListViewModel`, `CourseDetailsViewModel`, `EnrollmentListViewModel`

- [ ] 6.3 Views: `Index.cshtml` (card grid), `Details.cshtml` (accordion curriculum), `MyEnrollments.cshtml`

- [ ] 6.4 Gate: `[Authorize]` on `Enroll` and `MyEnrollments`; `Details` is public for published courses

---

## Phase 7 — Lesson & Content Module

**Goal:** Students can watch lessons, save position, download notes, mark complete.

- [ ] 7.1 `LessonsController`
  - `Watch(int id)` — access gate (enrolled OR isFreeSample), fetch Video + StudyNote + LessonProgress
  - `SavePosition(int lessonId, int positionSeconds)` POST (AJAX) — upsert LessonProgress
  - `MarkComplete(int lessonId)` POST — set status = Completed, trigger progress rollup
  - `DownloadNote(int noteId)` — return file, increment DailyActivityLog.notesDownloaded

- [ ] 7.2 Progress rollup on `MarkComplete`:
  1. Update `LessonProgress.status = Completed`, `completedAt = now`
  2. Recount completed lessons in module → update `ModuleProgress`
  3. Recount completed modules in course → update `CourseProgress.percentComplete`
  4. All three in one `TransactionScope`

- [ ] 7.3 `Watch.cshtml` — HTML5 `<video>` with JS resume from `videoPositionSeconds`; notes sidebar; download link; Mark Complete button

- [ ] 7.4 `progress.js` — save position on `video.pause` and `window.beforeunload` via `fetch`; animate progress bar width

---

## Phase 8 — Quiz Module

**Goal:** Students take quizzes, server grades, results shown.

- [ ] 8.1 `QuizController`
  - `Take(int quizId)` GET — check max attempts, load quiz with questions (shuffle options client-side)
  - `Submit(QuizSubmitViewModel vm)` POST — server grades all responses, save QuizAttempt + QuestionResponse rows, update DailyActivityLog
  - `Results(int attemptId)` GET — score, per-question breakdown

- [ ] 8.2 ViewModels: `QuizTakeViewModel` (questions without isCorrect flag), `QuizSubmitViewModel` (list of questionId + selectedOptionId), `QuizResultsViewModel`

- [ ] 8.3 `Take.cshtml` — question-by-question navigation driven by `quiz.js`; answer state held in JS object; timer countdown; single form POST on submit

- [ ] 8.4 `quiz.js`
  - Show one question at a time, [Back] / [Next] navigation
  - Track selected answers in object `{ questionId: optionId }`
  - Timer countdown (warn at 2 min remaining)
  - Serialize answers into hidden fields before form submit

- [ ] 8.5 `Results.cshtml` — score tile, pass/fail indicator, accordion per question showing correct answer and `isCorrect` snapshot

- [ ] 8.6 Max attempts guard: count existing QuizAttempts for (studentId, quizId) before allowing new one; show "No attempts remaining" if at limit

---

## Phase 9 — Student Dashboard

**Goal:** Student has a home base showing all progress, XP, streak, badges.

- [ ] 9.1 `ProgressController.Dashboard` — load enrollments + CourseProgress, recent QuizAttempts, UserBadges, User.totalXpPoints + streaks

- [ ] 9.2 `Dashboard.cshtml`
  - Greeting with streak + XP display
  - Enrolled courses grid with animated progress bar per course
  - Recent badges row
  - Recent quiz scores table

- [ ] 9.3 Streak calculation on each login: compare today's date with last `DailyActivityLog` entry; if consecutive increment `currentStreakDays`; update `longestStreakDays` if exceeded

---

## Phase 10 — Tutor Dashboard

**Goal:** Tutors can create courses, upload lessons, build quizzes, view student progress.

- [ ] 10.1 `TutorController` — gate with `[Authorize(Roles = "Tutor")]`
  - `Dashboard` — tutor's courses + enrollment counts
  - `CreateCourse GET/POST` — create COURSE row (isPublished = false)
  - `ManageCourse(int id) GET/POST` — edit course, add/reorder modules
  - `UploadLesson(int moduleId) GET/POST` — create Lesson + Video (file upload) + StudyNote
  - `CreateQuiz(int lessonId) GET/POST` — create Quiz + Questions + Options
  - `StudentProgress(int courseId)` — read-only COURSE_PROGRESS for enrolled students

- [ ] 10.2 File upload in `UploadLesson`
  - Accept `IFormFile`, validate MIME (`video/mp4`) and size (≤ 500 MB)
  - Save to `wwwroot/uploads/videos/{Guid}.mp4`
  - Insert `VIDEO` row with relative URL

- [ ] 10.3 Dynamic quiz builder in `CreateQuiz.cshtml`
  - JS: [Add Question] button clones a question template block
  - Each question block has a type selector; changing to MCQ reveals 4 option inputs with an "Is Correct" radio
  - All posted as array-indexed form fields: `Questions[0].Text`, `Questions[0].Options[0].Text`, etc.

---

## Phase 11 — Admin Panel

**Goal:** Admin has full CRUD over users, courses, guestbook; sees analytics.

- [ ] 11.1 `AdminController` — gate with `[Authorize(Roles = "Admin")]`
  - `Dashboard` — KPI tiles (total users, active enrollments, quizzes today, pending guestbook)
  - `Users GET` — paginated table; `EditUser POST` — change role, deactivate
  - `Courses GET` — course list; `PublishCourse POST`, `DeleteCourse POST`
  - `Guestbook GET` — pending entries; `Approve POST`, `Reject POST`
  - `Analytics GET` — basic stats: signups per month, most enrolled courses, daily active users

- [ ] 11.2 Pagination helper — generic `PagedList<T>` used across Users, Courses, Guestbook tables

- [ ] 11.3 Analytics page — render static summary tables (no charting library needed for phase 1); aggregate queries via raw LINQ

---

## Phase 12 — Guestbook Module

**Goal:** Anyone can submit; Admin moderates; approved entries show publicly.

- [ ] 12.1 `GuestbookController`
  - `Index` GET — all approved entries, newest first (paginated)
  - `Submit GET/POST` — create GuestbookEntry (status = Pending)

- [ ] 12.2 `Index.cshtml` — entry cards with guestName, message, submittedAt
- [ ] 12.3 `Submit.cshtml` — form: guestName (required), guestEmail (optional), message (required, 500 char max)
- [ ] 12.4 Server validation: strip HTML from message, check message length

---

## Phase 13 — Gamification

**Goal:** Badges auto-awarded; XP and streaks update on every action.

- [ ] 13.1 `BadgeService` (scoped service, not a repository) — `CheckAndAward(int userId)`:
  - Load all BADGE rows
  - For each badge, compute current user value for `badge.targetType`
  - If `currentValue >= badge.targetValue` AND no existing `USER_BADGE` row → insert UserBadge, add XP
  - Call from `ProgressController.MarkComplete` and `QuizController.Submit`

- [ ] 13.2 Seed 5 badges:
  | Name | targetType | targetValue |
  |---|---|---|
  | First Step | lessons_completed | 1 |
  | On a Roll | lessons_completed | 10 |
  | Quiz Ace | quizzes_passed | 5 |
  | Week Warrior | streak_days | 7 |
  | Course Finisher | courses_completed | 1 |

- [ ] 13.3 XP events — update `USER.totalXpPoints` and `DAILY_ACTIVITY_LOG.xpEarned`:
  | Action | XP |
  |---|---|
  | Lesson completed | +10 |
  | Quiz passed | +20 |
  | Daily login | +5 |
  | Badge earned | +badge.xpReward |

---

## Phase 14 — Notifications (Lightweight)

**Goal:** In-navbar notification count; dropdown list.

- [ ] 14.1 Add `Notification` entity (not in ERD — new migration):
  - `notificationID`, `userID FK`, `title`, `message`, `isRead bool`, `createdAt`

- [ ] 14.2 `NotificationHelper` static method — `Create(userId, title, message)` — used by controllers on events (badge earned, course published, session confirmed)

- [ ] 14.3 `_NavBar.cshtml` — `ViewComponent` that queries unread count for current user; renders count badge on bell icon + dropdown of last 5 messages

- [ ] 14.4 `MarkRead` AJAX endpoint — `POST /Notifications/MarkRead/{id}`

---

## Phase 15 — Polish & Hardening

**Goal:** Security, validation, error handling, responsive check.

- [ ] 15.1 Server-side validation on all POST actions — `ModelState.IsValid` guard on every controller action

- [ ] 15.2 Authorization audit — verify every controller action has correct `[Authorize]` attribute or explicit `[AllowAnonymous]`

- [ ] 15.3 Anti-CSRF — `@Html.AntiForgeryToken()` on every form; `[ValidateAntiForgeryToken]` on every POST action

- [ ] 15.4 Input sanitization — strip script tags from guestbook message and any rich-text fields; use `HtmlEncoder`

- [ ] 15.5 Custom error pages — `Error.cshtml` for 404 and 500; configure in `Program.cs`

- [ ] 15.6 Responsive test — check each page at 400px width: navbar collapses, course grid stacks, quiz takes full width, dashboard tiles stack

- [ ] 15.7 `prefers-reduced-motion` — wrap CSS transitions in `@media (prefers-reduced-motion: no-preference)`

- [ ] 15.8 Accessibility pass — every `<input>` has a `<label>`; every image has `alt`; focus rings visible; color contrast ≥ 4.5:1

---

## Phase 16 — Testing

**Goal:** Core logic covered; regressions caught.

- [ ] 16.1 Unit tests for `BadgeService.CheckAndAward` — mock repository, verify badge awarded at threshold

- [ ] 16.2 Unit tests for `QuizController.Submit` — verify server-side grading ignores client-sent `isCorrect`, grades from DB options

- [ ] 16.3 Unit tests for progress rollup — verify MODULE_PROGRESS and COURSE_PROGRESS update correctly when all lessons in a module complete

- [ ] 16.4 Integration test — register as Student, enroll in seeded course, complete lesson, check LessonProgress row exists

---

## Dependency Order Summary

```
Phase 1 (Scaffold)
  → Phase 2 (Entities + DB)
    → Phase 3 (Repositories)
      → Phase 4 (Auth)          Phase 5 (Layout)
        └──────────────────────────────────┐
                                           ▼
                             Phase 6 (Courses & Enrollment)
                               → Phase 7 (Lessons)
                                 → Phase 8 (Quiz)
                                   → Phase 9 (Student Dashboard)
                             Phase 10 (Tutor Dashboard)    ← parallel to 6-9
                             Phase 11 (Admin Panel)        ← parallel to 6-9
                             Phase 12 (Guestbook)          ← parallel to 6-9
                               → Phase 13 (Gamification)  ← needs 7,8
                                 → Phase 14 (Notifications)
                                   → Phase 15 (Polish)
                                     → Phase 16 (Testing)
```
