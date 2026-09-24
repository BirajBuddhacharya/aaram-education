# System Architecture

## Overview

Aaram Education follows a classic **3-layer ASP.NET Core MVC** architecture. All server rendering — no SPA framework. JavaScript handles interactivity only (quiz scoring, progress bars, form validation).

```
Browser
  │  HTTP request / form POST
  ▼
┌─────────────────────────────────────────┐
│         AaramEducation.Web              │
│  Controllers → Services → ViewModels    │
│  Razor Views (.cshtml) → HTML response  │
└───────────────┬─────────────────────────┘
                │ interfaces (ICourseRepository, etc.)
                ▼
┌─────────────────────────────────────────┐
│      AaramEducation.Infrastructure      │
│  EF Core Repositories                   │
│  ApplicationDbContext                   │
└───────────────┬─────────────────────────┘
                │ SQL
                ▼
          SQL Server / SQLite
```

---

## Layer Responsibilities

### AaramEducation.Web
- **Controllers** — receive HTTP, validate model state, call repositories, return Views or redirects.
- **ViewModels** — typed models passed to Razor Views; never expose raw entities to views.
- **wwwroot** — static assets: `aaram.css`, `quiz.js`, `validation.js`, uploaded files.
- **Program.cs** — DI wiring, Identity config, EF config, middleware pipeline.

### AaramEducation.Core
- Plain C# entity classes matching the ERD tables (no EF attributes — those live in Infrastructure via Fluent API).
- Interfaces that Infrastructure implements, so Web depends only on abstractions.
- Enums for role, status fields.

### AaramEducation.Infrastructure
- `ApplicationDbContext` with Fluent API configuration.
- Repository implementations using EF Core LINQ.
- `DbSeeder` for dev seed data.
- EF Core migrations.

---

## Authentication & Authorization

- **ASP.NET Core Identity** manages password hashing, login, session cookies.
- `UserRole` enum: `Student | Tutor | Admin`. Stored as string claim.
- `[Authorize(Roles = "Admin")]` on AdminController, `[Authorize(Roles = "Tutor")]` on TutorController.
- Guest access: unauthenticated users can view free-sample lessons (`LESSON.isFreeSample = true`) and guestbook.

---

## Request Flow Example — Student Takes a Quiz

```
1. GET /Quiz/Take/{quizId}
   → QuizController.Take(int quizId)
   → IQuizRepository.GetWithQuestions(quizId)
   → QuizTakeViewModel built
   → Take.cshtml rendered (questions hidden, revealed by quiz.js)

2. POST /Quiz/Submit
   → QuizController.Submit(QuizSubmitViewModel vm)
   → Server validates attempt count, quiz belongs to enrolled course
   → IQuizRepository.SaveAttempt(attempt)
   → IProgressRepository.UpdateDailyLog(userId, date)
   → Redirect → Results.cshtml

3. GET /Quiz/Results/{attemptId}
   → Shows score, per-question breakdown
```

---

## File Upload Flow (Tutor uploads lesson video)

```
POST /Tutor/UploadLesson  (multipart/form-data)
  → TutorController validates file type + size
  → Save to wwwroot/uploads/videos/{guid}.mp4
  → Insert VIDEO row with videoURL = "/uploads/videos/{guid}.mp4"
  → Redirect to ManageCourse
```

Phase 2: swap local storage for Azure Blob Storage by changing one service registration.

---

## Gamification

- Every meaningful action (lesson complete, quiz pass, daily login) writes to `DAILY_ACTIVITY_LOG`.
- `USER.totalXpPoints` and streak fields are cached values updated on write — avoids aggregating the log on every page load.
- `BADGE` rows define milestones. A background check after each action awards `USER_BADGE` if threshold crossed.

---

## Non-Functional Notes

| Concern | Approach |
|---|---|
| Security | Identity password hashing, CSRF tokens on all forms (`@Html.AntiForgeryToken()`), parameterised queries via EF Core |
| Responsive UI | Bootstrap 5 grid + custom `aaram.css` |
| Client validation | `validation.js` runs before POST; server re-validates all inputs |
| Performance | EF Core `.AsNoTracking()` on read-only queries; video served as static file |
| Accessibility | Semantic HTML5, `<label>` on every input, sufficient colour contrast |
