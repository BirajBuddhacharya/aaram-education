---
config:
  layout: elk
---
erDiagram
	direction TB

	USER {
		int userID PK
		string email UK
		string password_hash
		string firstName
		string lastName
		string profilePicture "nullable"
		datetime createdAt
		datetime updatedAt
		string role "student | admin"
		int totalXpPoints "gamification bonus"
		int currentStreakDays "cached from DAILY_ACTIVITY_LOG"
		int longestStreakDays "cached from DAILY_ACTIVITY_LOG"
	}

	ENROLLMENT {
		int enrollmentID PK
		int studentID FK "UNIQUE with courseID"
		int courseID FK "UNIQUE with studentID"
		datetime enrollmentDate
		string enrollmentStatus "active | completed | dropped"
	}

	COURSE {
		int courseID PK
		string courseName
		text courseDescription
		string subject
		string difficultyLevel
		boolean isPublished
		datetime createdAt
	}

	MODULE {
		int moduleID PK
		int courseID FK
		string moduleName
		text moduleDescription
		int sequenceOrder
		datetime createdAt
	}

	LESSON {
		int lessonID PK
		int moduleID FK
		string lessonTitle
		text lessonDescription
		int sequenceOrder
		boolean isFreeSample "guest-viewable preview lesson"
		datetime createdAt
	}

	LESSON_PROGRESS {
		int lessonProgressID PK
		int studentID FK "UNIQUE with lessonID"
		int lessonID FK "UNIQUE with studentID"
		string status "not_started | in_progress | completed"
		int videoPositionSeconds "resume point, nullable"
		int timeSpentSeconds
		datetime startedAt "nullable"
		datetime completedAt "nullable"
		datetime lastAccessedAt
	}

	MODULE_PROGRESS {
		int moduleProgressID PK
		int studentID FK "UNIQUE with moduleID"
		int moduleID FK "UNIQUE with studentID"
		string status "not_started | in_progress | completed"
		int lessonsCompleted "rollup cache of LESSON_PROGRESS"
		int totalLessons "rollup cache, denominator"
		datetime completedAt "nullable"
		datetime lastAccessedAt
	}

	COURSE_PROGRESS {
		int courseProgressID PK
		int enrollmentID FK,UK "1-to-1 with ENROLLMENT"
		string status "not_started | in_progress | completed"
		int lessonsCompleted "rollup cache of LESSON_PROGRESS"
		int modulesCompleted "rollup cache of MODULE_PROGRESS"
		float percentComplete "rollup cache"
		datetime completedAt "nullable"
		datetime lastAccessedAt
	}

	VIDEO {
		int videoID PK
		int lessonID FK
		string videoTitle
		string videoURL
		int durationSeconds
		datetime uploadedAt
	}

	STUDY_NOTE {
		int noteID PK
		int lessonID FK
		string noteTitle
		text noteContent
		string fileURL "downloadable file"
		datetime createdAt
	}

	QUIZ {
		int quizID PK
		int lessonID FK
		string quizTitle
		text quizDescription
		int passingScore
		int maxAttempts
		datetime createdAt
	}

	QUIZ_QUESTION {
		int questionID PK
		int quizID FK
		text questionText
		string questionType "multiple_choice | true_false | short_answer"
		int sequenceOrder
		int pointsValue
	}

	ANSWER_OPTION {
		int optionID PK
		int questionID FK
		text optionText
		boolean isCorrect
		int sequenceOrder
	}

	QUIZ_ATTEMPT {
		int attemptID PK
		int studentID FK
		int quizID FK
		datetime attemptDate
		int scoreAchieved
		int timeTakenSeconds
		string attemptStatus "in_progress | submitted | graded"
	}

	QUESTION_RESPONSE {
		int responseID PK
		int attemptID FK "UNIQUE with questionID"
		int questionID FK "UNIQUE with attemptID"
		int selectedOptionID FK "nullable - option-based questions only"
		text textResponse "nullable - short_answer questions only"
		boolean isCorrect "snapshot at grading time"
		datetime answeredAt
	}

	GUESTBOOK_ENTRY {
		int entryID PK
		string guestName
		string guestEmail "nullable"
		text message
		datetime submittedAt
		string moderationStatus "pending | approved | rejected"
		int moderatedBy FK "nullable - admin USER"
		datetime moderatedAt "nullable"
	}

	BADGE {
		int badgeID PK
		string badgeName
		text description
		string iconURL
		int xpReward
		string targetType
		int targetValue
	}

	USER_BADGE {
		int userBadgeID PK
		int userID FK "UNIQUE with badgeID"
		int badgeID FK "UNIQUE with userID"
		datetime earnedAt
	}

	DAILY_ACTIVITY_LOG {
		int logID PK
		int userID FK "UNIQUE with activityDate"
		date activityDate "UNIQUE with userID"
		int loginCount
		int lessonsStarted
		int lessonsCompleted
		int quizzesAttempted
		int quizzesPassed
		int notesDownloaded
		int videoWatchSeconds
		int totalTimeSeconds
		int xpEarned "gamification bonus"
	}

	USER ||--o{ ENROLLMENT : "enrolls"
	COURSE ||--o{ ENROLLMENT : "has"
	COURSE ||--o{ MODULE : "contains"
	MODULE ||--o{ LESSON : "contains"
	LESSON ||--o{ VIDEO : "includes"
	LESSON ||--o{ STUDY_NOTE : "includes"
	LESSON ||--o{ QUIZ : "includes"
	USER ||--o{ LESSON_PROGRESS : "progresses through"
	LESSON ||--o{ LESSON_PROGRESS : "tracked by"
	USER ||--o{ MODULE_PROGRESS : "progresses through"
	MODULE ||--o{ MODULE_PROGRESS : "tracked by"
	ENROLLMENT ||--|| COURSE_PROGRESS : "summarised by"
	USER ||--o{ QUIZ_ATTEMPT : "takes"
	QUIZ ||--o{ QUIZ_ATTEMPT : "has"
	QUIZ ||--o{ QUIZ_QUESTION : "contains"
	QUIZ_QUESTION ||--o{ ANSWER_OPTION : "has"
	QUIZ_ATTEMPT ||--o{ QUESTION_RESPONSE : "records"
	QUIZ_QUESTION ||--o{ QUESTION_RESPONSE : "answered by"
	ANSWER_OPTION |o--o{ QUESTION_RESPONSE : "selected in"
	USER |o--o{ GUESTBOOK_ENTRY : "moderates"
	USER ||--o{ USER_BADGE : "earns"
	BADGE ||--o{ USER_BADGE : "awards"
	USER ||--o{ DAILY_ACTIVITY_LOG : "logs"
