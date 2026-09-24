# Modules — Entity Mapping & CRUD Matrix

Each module maps to one or more controllers. Entities listed are those the module **owns** (creates/updates/deletes). Other entities are read-only from that module's perspective.

---

## 1. Auth Module

**Controller:** `AccountController`  
**Roles:** All (register as Student or Tutor; Admin created via seed)

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| USER | ✓ | ✓ | ✓ | Admin only | Password hashed by Identity |

### Pages
- `Register.cshtml` — creates USER, role = Student or Tutor
- `Login.cshtml` — authenticates, issues cookie
- `Profile.cshtml` — updates name, email, profile picture

### Validation
- Client: `validation.js` — required fields, email format, password strength
- Server: unique email check, Identity password rules

---

## 2. Course Catalog Module

**Controller:** `CoursesController`  
**Roles:** Guest (read free samples), Student (enroll), Tutor (CRUD own courses), Admin (publish/unpublish)

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| COURSE | Tutor | All | Tutor/Admin | Admin | Tutor creates; Admin publishes |
| MODULE | Tutor | All | Tutor | Tutor | Ordered within course |
| ENROLLMENT | Student | Student | — | Student | Student self-enroll/drop |

### Pages
- `Index.cshtml` — course catalog grid, filter by subject/difficulty
- `Details.cshtml` — course overview, module list, enroll CTA
- `MyEnrollments.cshtml` — student's enrolled courses + status

### Business Rules
- Guest can view courses where `isPublished = true`.
- Free lesson preview: up to 1 lesson per module where `isFreeSample = true` — no enrollment needed.
- Duplicate enrollment rejected: unique constraint on (studentID, courseID).

---

## 3. Lesson & Content Module

**Controller:** `LessonsController`  
**Roles:** Student (watch, download), Tutor (upload, edit)

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| LESSON | Tutor | Student/Tutor | Tutor | Tutor | Must be enrolled or isFreeSample |
| VIDEO | Tutor | Student/Tutor | Tutor | Tutor | File upload to wwwroot/uploads/videos/ |
| STUDY_NOTE | Tutor | Student/Tutor | Tutor | Tutor | File or inline HTML |

### Pages
- `Watch.cshtml` — video player, notes sidebar, download link, "Mark Complete" button
- `Complete.cshtml` — confirmation + next lesson prompt

### Business Rules
- Access gate: `ENROLLMENT.enrollmentStatus = active` OR `LESSON.isFreeSample = true`.
- Mark complete: writes `LESSON_PROGRESS.status = completed`, triggers MODULE_PROGRESS and COURSE_PROGRESS rollup.
- Video position saved on pause/exit to `LESSON_PROGRESS.videoPositionSeconds`.

---

## 4. Quiz Module

**Controller:** `QuizController`  
**Roles:** Student (take quiz), Tutor (create/edit quiz), Admin (read all)

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| QUIZ | Tutor | Student/Tutor | Tutor | Tutor | One quiz per lesson |
| QUIZ_QUESTION | Tutor | Student/Tutor | Tutor | Tutor | |
| ANSWER_OPTION | Tutor | Student/Tutor | Tutor | Tutor | isCorrect set by tutor |
| QUIZ_ATTEMPT | Student | Student | — | — | Immutable after submit |
| QUESTION_RESPONSE | Student | Student | — | — | Immutable after submit |

### Pages
- `Take.cshtml` — question-by-question UI driven by `quiz.js`, timer display
- `Results.cshtml` — score, pass/fail, per-question breakdown with correct answer

### Business Rules
- Max attempts enforced: count existing `QUIZ_ATTEMPT` rows for (studentID, quizID) before allowing new one.
- Grading: server-side only — `QUESTION_RESPONSE.isCorrect` set on submit, never trusted from client.
- `DAILY_ACTIVITY_LOG.quizzesAttempted++` and `quizzesPassed++` (if score ≥ passingScore) on submit.

---

## 5. Progress Module

**Controller:** `ProgressController`  
**Roles:** Student (own data only), Tutor (students enrolled in their courses), Admin (all)

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| LESSON_PROGRESS | auto | Student | auto | — | Written by LessonsController on watch/complete |
| MODULE_PROGRESS | auto | Student | auto | — | Rollup updated by LessonsController |
| COURSE_PROGRESS | auto | Student | auto | — | Rollup updated by LessonsController |
| DAILY_ACTIVITY_LOG | auto | Student | auto | — | Upsert on every action |

### Pages
- `Dashboard.cshtml` — enrolled courses + % complete, recent activity, XP total, streak, badges earned

### Business Rules
- Progress rows created on first lesson access (not on enrollment).
- Rollup update order: LESSON_PROGRESS → MODULE_PROGRESS → COURSE_PROGRESS — all in one DB transaction.
- Streak: compare `DAILY_ACTIVITY_LOG.activityDate` for consecutive days; update `USER.currentStreakDays`.

---

## 6. Gamification Module

**No dedicated controller** — badge checks run inside ProgressController and QuizController after state changes.

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| BADGE | Admin seed | All | Admin | Admin | Milestone definitions |
| USER_BADGE | auto | Student | — | — | Awarded when threshold crossed |

### Business Rules
- After any XP-earning action: check all BADGE rows for `targetType` match and `targetValue ≤ current count`.
- Award `USER_BADGE` and add `BADGE.xpReward` to `USER.totalXpPoints`.
- Each badge awarded once — unique constraint on (userID, badgeID).

---

## 7. Tutor Dashboard Module

**Controller:** `TutorController`  
**Roles:** Tutor only

### Pages
- `Dashboard.cshtml` — summary of own courses, enrolled student counts
- `ManageCourse.cshtml` — edit course metadata, add/reorder modules
- `UploadLesson.cshtml` — create lesson, upload VIDEO, attach STUDY_NOTE
- `CreateQuiz.cshtml` — build QUIZ with QUIZ_QUESTIONs and ANSWER_OPTIONs
- `StudentProgress.cshtml` — read-only view of COURSE_PROGRESS for own course's students

### Business Rules
- Tutors can only edit their own courses (`COURSE.createdByUserID` — add this FK if not in ERD).
- File upload: validate MIME type (`video/mp4`, `application/pdf`) and max size (500 MB video, 20 MB note) before save.

---

## 8. Admin Panel Module

**Controller:** `AdminController`  
**Roles:** Admin only

### CRUD Matrix (Admin has full access to all entities)

| Entity | Create | Read | Update | Delete |
|---|---|---|---|---|
| USER | ✓ | ✓ | ✓ | ✓ (soft: deactivate) |
| COURSE | ✓ | ✓ | ✓ (publish) | ✓ |
| LESSON | — | ✓ | — | ✓ |
| QUIZ | — | ✓ | — | ✓ |
| GUESTBOOK_ENTRY | — | ✓ | ✓ (moderate) | ✓ |
| BADGE | ✓ | ✓ | ✓ | ✓ |
| DAILY_ACTIVITY_LOG | — | ✓ | — | — |

### Pages
- `Dashboard.cshtml` — KPIs: total users, active enrollments, quizzes taken today
- `Users.cshtml` — paginated user table, role change, deactivate
- `Courses.cshtml` — course list, publish/unpublish toggle
- `Guestbook.cshtml` — pending entries queue, approve/reject
- `Analytics.cshtml` — charts: signups over time, most popular courses, daily active users

---

## 9. Guestbook Module

**Controller:** `GuestbookController`  
**Roles:** Anyone (submit), Admin (moderate)

### Owned Entities
| Entity | C | R | U | D | Notes |
|---|---|---|---|---|---|
| GUESTBOOK_ENTRY | Any | Admin/Public (approved) | Admin (status) | Admin | Pending by default |

### Pages
- `Index.cshtml` — shows all `approved` entries; registered users shown by name from USER
- `Submit.cshtml` — form: guestName, guestEmail (optional), message

### Business Rules
- New entries default to `moderationStatus = pending`.
- `moderatedBy` and `moderatedAt` set when Admin acts.
- Public view only shows `approved` entries.

---

## 10. Notification (lightweight, no dedicated module)

Notifications are simple row inserts triggered by backend events:

| Event | Notification text |
|---|---|
| Tutor approves session | "Your session with [Tutor] is confirmed for [date]" |
| Admin approves course | "Your course '[name]' is now published" |
| Badge earned | "You earned the '[badge]' badge!" |
| Quiz passed | "You passed [quiz]! Score: [X]%" |

Read via a partial in `_NavBar.cshtml` — unread count badge + dropdown list.  
No dedicated entity in current ERD — implement as a simple `NOTIFICATION` table (userID, title, message, isRead, createdAt) added in migration.
