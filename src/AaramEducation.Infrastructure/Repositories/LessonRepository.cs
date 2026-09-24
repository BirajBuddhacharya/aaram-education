using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class LessonRepository(ApplicationDbContext db) : ILessonRepository
{
    public Task<Lesson?> GetByIdAsync(int lessonId) =>
        db.Lessons.AsNoTracking().FirstOrDefaultAsync(l => l.LessonId == lessonId);

    public Task<Lesson?> GetWithContentAsync(int lessonId) =>
        db.Lessons.AsNoTracking()
            .Include(l => l.Videos)
            .Include(l => l.StudyNotes)
            .Include(l => l.Quizzes)
            .Include(l => l.Module).ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

    public async Task<IEnumerable<Lesson>> GetByModuleAsync(int moduleId) =>
        await db.Lessons.AsNoTracking()
            .Where(l => l.ModuleId == moduleId)
            .OrderBy(l => l.SequenceOrder)
            .ToListAsync();

    public async Task<Lesson> CreateAsync(Lesson lesson)
    {
        db.Lessons.Add(lesson);
        await db.SaveChangesAsync();
        return lesson;
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        db.Lessons.Update(lesson);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int lessonId)
    {
        var lesson = await db.Lessons.FindAsync(lessonId);
        if (lesson is not null)
        {
            db.Lessons.Remove(lesson);
            await db.SaveChangesAsync();
        }
    }

    public Task<Video?> GetVideoAsync(int lessonId) =>
        db.Videos.AsNoTracking().FirstOrDefaultAsync(v => v.LessonId == lessonId);

    public Task<StudyNote?> GetStudyNoteAsync(int noteId) =>
        db.StudyNotes.AsNoTracking().FirstOrDefaultAsync(n => n.NoteId == noteId);

    public async Task<Video> SaveVideoAsync(Video video)
    {
        db.Videos.Add(video);
        await db.SaveChangesAsync();
        return video;
    }

    public async Task<StudyNote> SaveStudyNoteAsync(StudyNote note)
    {
        db.StudyNotes.Add(note);
        await db.SaveChangesAsync();
        return note;
    }
}
