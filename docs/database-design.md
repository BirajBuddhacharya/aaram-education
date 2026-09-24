# Database Design

Source of truth: [`ERD.md`](../ERD.md) — Mermaid diagram with all relationships.

Stack: **Entity Framework Core 8** against **SQL Server** (dev: SQLite).  
Config: Fluent API in `ApplicationDbContext`. No data-annotation attributes on entity classes.

---

## Entity Definitions

### USER
Central identity table. Roles are stored as a string enum on this table — no separate roles table needed.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| userID | int | PK, identity | |
| email | string(256) | UK, NOT NULL | Login identifier |
| password_hash | string | NOT NULL | ASP.NET Identity hash |
| firstName | string(100) | NOT NULL | |
| lastName | string(100) | NOT NULL | |
| profilePicture | string | nullable | Relative URL to upload |
| createdAt | datetime | NOT NULL, default now | |
| updatedAt | datetime | NOT NULL | |
| role | string | NOT NULL | `student \| tutor \| admin` |
| totalXpPoints | int | NOT NULL, default 0 | Cached XP sum |
| currentStreakDays | int | NOT NULL, default 0 | Cached from DAILY_ACTIVITY_LOG |
| longestStreakDays | int | NOT NULL, default 0 | Cached from DAILY_ACTIVITY_LOG |

---

### COURSE
Created and managed by tutors; browsed by students.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| courseID | int | PK | |
| courseName | string(200) | NOT NULL | |
| courseDescription | text | NOT NULL | |
| subject | string(100) | NOT NULL | Maths, Science, English, Computing |
| difficultyLevel | string | NOT NULL | beginner / intermediate / advanced |
| isPublished | bool | NOT NULL, default false | Admin publishes |
| createdAt | datetime | NOT NULL | |

---

### MODULE
Groups lessons inside a course. Allows ordered curriculum structure.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| moduleID | int | PK | |
| courseID | int | FK → COURSE | NOT NULL |
| moduleName | string(200) | NOT NULL | |
| moduleDescription | text | nullable | |
| sequenceOrder | int | NOT NULL | Determines display order |
| createdAt | datetime | NOT NULL | |

---

### LESSON

| Column | Type | Constraints | Notes |
|---|---|---|---|
| lessonID | int | PK | |
| moduleID | int | FK → MODULE | NOT NULL |
| lessonTitle | string(200) | NOT NULL | |
| lessonDescription | text | nullable | |
| sequenceOrder | int | NOT NULL | |
| isFreeSample | bool | NOT NULL, default false | Viewable without enrollment |
| createdAt | datetime | NOT NULL | |

---

### VIDEO

| Column | Type | Constraints | Notes |
|---|---|---|---|
| videoID | int | PK | |
| lessonID | int | FK → LESSON | NOT NULL |
| videoTitle | string(200) | NOT NULL | |
| videoURL | string | NOT NULL | Relative path in wwwroot/uploads/videos/ |
| durationSeconds | int | NOT NULL | |
| uploadedAt | datetime | NOT NULL | |

---

### STUDY_NOTE
Downloadable notes per lesson.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| noteID | int | PK | |
| lessonID | int | FK → LESSON | NOT NULL |
| noteTitle | string(200) | NOT NULL | |
| noteContent | text | nullable | Inline HTML content |
| fileURL | string | nullable | Downloadable PDF/DOCX path |
| createdAt | datetime | NOT NULL | |

---

### ENROLLMENT
Junction between student and course. Unique on (studentID, courseID).

| Column | Type | Constraints | Notes |
|---|---|---|---|
| enrollmentID | int | PK | |
| studentID | int | FK → USER | NOT NULL |
| courseID | int | FK → COURSE | NOT NULL |
| enrollmentDate | datetime | NOT NULL | |
| enrollmentStatus | string | NOT NULL | `active \| completed \| dropped` |

Unique constraint: `(studentID, courseID)`.

---

### LESSON_PROGRESS
Per-student per-lesson tracking. Unique on (studentID, lessonID).

| Column | Type | Constraints | Notes |
|---|---|---|---|
| lessonProgressID | int | PK | |
| studentID | int | FK → USER | NOT NULL |
| lessonID | int | FK → LESSON | NOT NULL |
| status | string | NOT NULL | `not_started \| in_progress \| completed` |
| videoPositionSeconds | int | nullable | Resume point |
| timeSpentSeconds | int | NOT NULL, default 0 | |
| startedAt | datetime | nullable | |
| completedAt | datetime | nullable | |
| lastAccessedAt | datetime | NOT NULL | |

---

### MODULE_PROGRESS
Rollup cache per student per module.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| moduleProgressID | int | PK | |
| studentID | int | FK → USER | NOT NULL |
| moduleID | int | FK → MODULE | NOT NULL |
| status | string | NOT NULL | `not_started \| in_progress \| completed` |
| lessonsCompleted | int | NOT NULL, default 0 | Cached rollup |
| totalLessons | int | NOT NULL | Cached denominator |
| completedAt | datetime | nullable | |
| lastAccessedAt | datetime | NOT NULL | |

---

### COURSE_PROGRESS
1-to-1 with ENROLLMENT. Top-level rollup.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| courseProgressID | int | PK | |
| enrollmentID | int | FK → ENROLLMENT, UK | 1-to-1 |
| status | string | NOT NULL | `not_started \| in_progress \| completed` |
| lessonsCompleted | int | NOT NULL, default 0 | |
| modulesCompleted | int | NOT NULL, default 0 | |
| percentComplete | float | NOT NULL, default 0.0 | |
| completedAt | datetime | nullable | |
| lastAccessedAt | datetime | NOT NULL | |

---

### QUIZ

| Column | Type | Constraints | Notes |
|---|---|---|---|
| quizID | int | PK | |
| lessonID | int | FK → LESSON | NOT NULL |
| quizTitle | string(200) | NOT NULL | |
| quizDescription | text | nullable | |
| passingScore | int | NOT NULL | Percentage e.g. 70 |
| maxAttempts | int | NOT NULL, default 3 | |
| createdAt | datetime | NOT NULL | |

---

### QUIZ_QUESTION

| Column | Type | Constraints | Notes |
|---|---|---|---|
| questionID | int | PK | |
| quizID | int | FK → QUIZ | NOT NULL |
| questionText | text | NOT NULL | |
| questionType | string | NOT NULL | `multiple_choice \| true_false \| short_answer` |
| sequenceOrder | int | NOT NULL | |
| pointsValue | int | NOT NULL, default 1 | |

---

### ANSWER_OPTION
Options for MCQ and true/false questions.

| Column | Type | Constraints | Notes |
|---|---|---|---|
| optionID | int | PK | |
| questionID | int | FK → QUIZ_QUESTION | NOT NULL |
| optionText | text | NOT NULL | |
| isCorrect | bool | NOT NULL | |
| sequenceOrder | int | NOT NULL | |

---

### QUIZ_ATTEMPT

| Column | Type | Constraints | Notes |
|---|---|---|---|
| attemptID | int | PK | |
| studentID | int | FK → USER | NOT NULL |
| quizID | int | FK → QUIZ | NOT NULL |
| attemptDate | datetime | NOT NULL | |
| scoreAchieved | int | NOT NULL, default 0 | |
| timeTakenSeconds | int | NOT NULL, default 0 | |
| attemptStatus | string | NOT NULL | `in_progress \| submitted \| graded` |

---

### QUESTION_RESPONSE
Per-question answer within one attempt. Unique on (attemptID, questionID).

| Column | Type | Constraints | Notes |
|---|---|---|---|
| responseID | int | PK | |
| attemptID | int | FK → QUIZ_ATTEMPT | NOT NULL |
| questionID | int | FK → QUIZ_QUESTION | NOT NULL |
| selectedOptionID | int | FK → ANSWER_OPTION, nullable | MCQ/T-F only |
| textResponse | text | nullable | short_answer only |
| isCorrect | bool | NOT NULL | Snapshot at grading time |
| answeredAt | datetime | NOT NULL | |

---

### GUESTBOOK_ENTRY

| Column | Type | Constraints | Notes |
|---|---|---|---|
| entryID | int | PK | |
| guestName | string(100) | NOT NULL | |
| guestEmail | string(256) | nullable | |
| message | text | NOT NULL | |
| submittedAt | datetime | NOT NULL | |
| moderationStatus | string | NOT NULL | `pending \| approved \| rejected` |
| moderatedBy | int | FK → USER, nullable | Admin who reviewed |
| moderatedAt | datetime | nullable | |

---

### BADGE

| Column | Type | Constraints | Notes |
|---|---|---|---|
| badgeID | int | PK | |
| badgeName | string(100) | NOT NULL, UK | |
| description | text | NOT NULL | |
| iconURL | string | NOT NULL | |
| xpReward | int | NOT NULL | XP credited on award |
| targetType | string | NOT NULL | e.g. `lessons_completed`, `streak_days` |
| targetValue | int | NOT NULL | Threshold to earn badge |

---

### USER_BADGE
Junction — unique on (userID, badgeID).

| Column | Type | Constraints | Notes |
|---|---|---|---|
| userBadgeID | int | PK | |
| userID | int | FK → USER | NOT NULL |
| badgeID | int | FK → BADGE | NOT NULL |
| earnedAt | datetime | NOT NULL | |

---

### DAILY_ACTIVITY_LOG
One row per user per calendar day. Unique on (userID, activityDate).

| Column | Type | Constraints | Notes |
|---|---|---|---|
| logID | int | PK | |
| userID | int | FK → USER | NOT NULL |
| activityDate | date | NOT NULL | |
| loginCount | int | NOT NULL, default 0 | |
| lessonsStarted | int | NOT NULL, default 0 | |
| lessonsCompleted | int | NOT NULL, default 0 | |
| quizzesAttempted | int | NOT NULL, default 0 | |
| quizzesPassed | int | NOT NULL, default 0 | |
| notesDownloaded | int | NOT NULL, default 0 | |
| videoWatchSeconds | int | NOT NULL, default 0 | |
| totalTimeSeconds | int | NOT NULL, default 0 | |
| xpEarned | int | NOT NULL, default 0 | |

Unique constraint: `(userID, activityDate)`.

---

## Relationship Summary

```
USER ──< ENROLLMENT >── COURSE ──< MODULE ──< LESSON ──< VIDEO
                                                      ──< STUDY_NOTE
                                                      ──< QUIZ ──< QUIZ_QUESTION ──< ANSWER_OPTION
USER ──< LESSON_PROGRESS >── LESSON
USER ──< MODULE_PROGRESS >── MODULE
ENROLLMENT ──1 COURSE_PROGRESS
USER ──< QUIZ_ATTEMPT >── QUIZ
QUIZ_ATTEMPT ──< QUESTION_RESPONSE >── QUIZ_QUESTION
USER ──< USER_BADGE >── BADGE
USER ──< DAILY_ACTIVITY_LOG
USER |o── GUESTBOOK_ENTRY (moderated by, nullable)
```

Full Mermaid diagram: [`ERD.md`](../ERD.md)
