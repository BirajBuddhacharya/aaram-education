# Frontend Design — UI Correlation with Design Docs

Correlates Razor Views with the ERD entities and the design reference in `design/`.

---

## Design Principles (from proposal)

- **Calm, uncluttered interface** — generous whitespace, restrained colour palette, clear typography
- **Stress-free** — minimum clicks to reach a lesson; no pop-ups or hard-sell
- **Responsive** — Bootstrap 5 grid; tested on desktop, tablet, mobile

---

## Layout Shell (`_Layout.cshtml`)

```
┌────────────────────────────────────────────────────┐
│  _NavBar.cshtml                                    │
│  Logo | Courses | About | Guestbook | [Login/User] │
│  [Unread notifications badge for logged-in users]  │
├────────────────────────────────────────────────────┤
│                                                    │
│  @RenderBody()                                     │
│                                                    │
├────────────────────────────────────────────────────┤
│  _Footer.cshtml                                    │
│  Links: Home | About | Guestbook | Privacy         │
└────────────────────────────────────────────────────┘
```

**CSS vars in `aaram.css`:**
```css
:root {
  --aaram-bg:      #F9F7F4;   /* warm off-white — calm, non-harsh */
  --aaram-surface: #FFFFFF;
  --aaram-primary: #4A7C59;   /* muted green — growth, learning */
  --aaram-accent:  #E8A838;   /* warm amber — progress highlights */
  --aaram-text:    #2D2D2D;
  --aaram-muted:   #6B7280;
  --aaram-radius:  8px;
}
```

---

## Page-by-Page Breakdown

### Home — `Index.cshtml`

**Entities used:** COURSE (published, limit 6), USER (login state)

```
┌──────────────────────────────────────────────────┐
│  Hero section                                    │
│  "Learn at your own pace. No stress."           │
│  [Start Learning] [Browse Courses]               │
├──────────────────────────────────────────────────┤
│  Subject filter tabs: All | Maths | Science |   │
│  English | Computing                             │
├──────────────────────────────────────────────────┤
│  Course cards grid (3-col desktop, 1-col mobile) │
│  Card: thumbnail | subject tag | title | tutor   │
│        difficulty badge | [Enroll / View]        │
└──────────────────────────────────────────────────┘
```

---

### Course Catalog — `Courses/Index.cshtml`

**Entities used:** COURSE, MODULE (count), ENROLLMENT (student's status)

```
Sidebar filters:            Course grid:
  Subject                   [Card] [Card] [Card]
  Difficulty                [Card] [Card] [Card]
  Status (enrolled/all)     Pagination
```

---

### Course Details — `Courses/Details.cshtml`

**Entities used:** COURSE, MODULE, LESSON (list), ENROLLMENT

```
┌───────────────────────────────────────────────┐
│  Course banner: title, subject, difficulty    │
│  [Enroll Now] or [Continue Learning]          │
├───────────────────────────────────────────────┤
│  About this course (description)             │
├───────────────────────────────────────────────┤
│  Curriculum accordion                         │
│  ▼ Module 1: Introduction                    │
│     ○ Lesson 1.1 — Free Preview  [Watch]     │
│     🔒 Lesson 1.2              [Enroll]      │
│  ▼ Module 2: Core Concepts                   │
│     🔒 Lesson 2.1                            │
└───────────────────────────────────────────────┘
```

Free sample lessons (`isFreeSample = true`) shown with `[Watch]` regardless of enrollment.

---

### Lesson Watch — `Lessons/Watch.cshtml`

**Entities used:** LESSON, VIDEO, STUDY_NOTE, LESSON_PROGRESS, MODULE (breadcrumb)

```
Breadcrumb: Course › Module › Lesson

┌──────────────────────────┬───────────────────┐
│                          │  Notes sidebar    │
│   <video> player         │  Inline summary   │
│   HTML5 native controls  │  [Download PDF]   │
│   Resume from saved pos  │                   │
│                          │  Progress: 3/8    │
│                          │  lessons done     │
└──────────────────────────┴───────────────────┘
[← Previous Lesson]           [Mark Complete →]
                    [Take Quiz]
```

`progress.js` animates the lesson progress bar.  
Video position saved on `pause` and `beforeunload` via `fetch POST /Lessons/SavePosition`.

---

### Quiz Take — `Quiz/Take.cshtml`

**Entities used:** QUIZ, QUIZ_QUESTION, ANSWER_OPTION

```
Quiz: "Chapter 1 Quiz" — 10 questions

┌─────────────────────────────────────────────┐
│ Question 3 of 10              ⏱ 12:45 left  │
│                                             │
│ Which of the following is...?              │
│                                             │
│ ○ Option A                                  │
│ ● Option B    ← selected                   │
│ ○ Option C                                  │
│ ○ Option D                                  │
│                                             │
│ [← Back]                     [Next →]      │
│                   [Submit Quiz]             │
└─────────────────────────────────────────────┘
```

`quiz.js` handles: client-side navigation between questions, answer state tracking, timer countdown.  
Submit sends all answers in one POST — server grades server-side.

---

### Quiz Results — `Quiz/Results.cshtml`

**Entities used:** QUIZ_ATTEMPT, QUESTION_RESPONSE, ANSWER_OPTION

```
Score: 8 / 10 (80%) ✓ Passed

Progress bar: ████████░░

Question 1 ✓  Your answer: Option B  (correct)
Question 2 ✗  Your answer: Option A  Correct: Option C
               Explanation: Because...

[Retake Quiz]  [Back to Lesson]  [Next Lesson →]
```

---

### Student Dashboard — `Progress/Dashboard.cshtml`

**Entities used:** ENROLLMENT, COURSE_PROGRESS, USER (XP, streak), USER_BADGE, QUIZ_ATTEMPT (recent)

```
Welcome back, Sameer 👋   🔥 5-day streak   ⭐ 420 XP

┌──────────────┬──────────────┬──────────────┐
│ My Courses   │ Recent Badges│ Recent Quizzes│
│              │              │               │
│ Maths Yr10   │ 🏅 First     │ Ch1 Quiz  80% │
│ ████░░ 60%   │  Lesson Done │ Ch2 Quiz  90% │
│ [Continue]   │ 🏅 7-Day     │               │
│              │  Streak      │               │
│ Science Yr11 │              │               │
│ ██░░░░ 30%   │              │               │
│ [Continue]   │              │               │
└──────────────┴──────────────┴───────────────┘
```

---

### Tutor Dashboard — `Tutor/Dashboard.cshtml`

**Entities used:** COURSE (own), ENROLLMENT (counts), LESSON_PROGRESS (aggregate)

```
My Courses

┌─────────────────────────────────────────────┐
│ Maths Yr10 — 42 students enrolled           │
│ Avg progress: 58%  [Manage] [View Students] │
├─────────────────────────────────────────────┤
│ Science Yr11 — 28 students enrolled         │
│ Avg progress: 34%  [Manage] [View Students] │
└─────────────────────────────────────────────┘
[+ Create New Course]
```

---

### Admin Dashboard — `Admin/Dashboard.cshtml`

**Entities used:** All (aggregate reads)

```
┌────────┬────────────┬────────────┬────────────┐
│ 1,240  │ 89 active  │ 342 quizzes│ 12 pending │
│ users  │ enrollments│ today      │ guestbook  │
└────────┴────────────┴────────────┴────────────┘

Pending guestbook entries: [Review]
Unpublished courses: [Review]
Recent signups: [table]
```

---

### Guestbook — `Guestbook/Index.cshtml`

**Entities used:** GUESTBOOK_ENTRY (approved only)

```
Guestbook

┌─────────────────────────────────────┐
│ "Great platform!" — Priya S.        │
│ 2 days ago                          │
├─────────────────────────────────────┤
│ "Helped me pass my exams!" — Raman  │
│ 1 week ago                          │
└─────────────────────────────────────┘

[Share your experience →]
```

---

## JavaScript Files

| File | Responsibilities |
|---|---|
| `quiz.js` | Question navigation state, answer selection, timer countdown, form submit |
| `progress.js` | Animated progress bars (CSS width transition), video position save on pause |
| `validation.js` | Required fields, email regex, password strength meter, real-time feedback before POST |

No bundler — files loaded via `<script src="~/js/quiz.js">` where needed per view.

---

## Responsive Breakpoints

Using Bootstrap 5 defaults:
- `< 576px` — mobile: single column, hamburger nav
- `576–992px` — tablet: 2-column course grid
- `> 992px` — desktop: 3-column grid, sidebar visible

Custom `aaram.css` overrides Bootstrap where needed to maintain the calm, low-density aesthetic.
