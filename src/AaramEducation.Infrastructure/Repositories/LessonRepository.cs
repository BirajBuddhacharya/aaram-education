using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;

namespace AaramEducation.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _db;
        public LessonRepository(ApplicationDbContext db) { _db = db; }

        public Task<Lesson?> GetByIdAsync(int lessonId) =>
            _db.Lessons.AsNoTracking().FirstOrDefaultAsync(l => l.LessonId == lessonId);

        public Task<Lesson?> GetWithContentAsync(int lessonId) =>
            _db.Lessons.AsNoTracking()
                .Include(l => l.Videos)
                .Include(l => l.StudyNotes)
                .Include(l => l.Quizzes)
                .Include(l => l.Module.Course.CreatedBy)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        public async Task<IEnumerable<Lesson>> GetByModuleAsync(int moduleId) =>
            await _db.Lessons.AsNoTracking()
                .Include(l => l.Videos)
                .Where(l => l.ModuleId == moduleId)
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _db.Lessons.Add(lesson);
            await _db.SaveChangesAsync();
            return lesson;
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _db.Entry(lesson).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int lessonId)
        {
            var lesson = await _db.Lessons.FindAsync(lessonId);
            if (lesson is not null)
            {
                _db.Lessons.Remove(lesson);
                await _db.SaveChangesAsync();
            }
        }

        public Task<Video?> GetVideoAsync(int lessonId) =>
            _db.Videos.AsNoTracking().FirstOrDefaultAsync(v => v.LessonId == lessonId);

        public Task<StudyNote?> GetStudyNoteAsync(int noteId) =>
            _db.StudyNotes.AsNoTracking().FirstOrDefaultAsync(n => n.NoteId == noteId);

        public async Task<Video> SaveVideoAsync(Video video)
        {
            _db.Videos.Add(video);
            await _db.SaveChangesAsync();
            return video;
        }

        public async Task<StudyNote> SaveStudyNoteAsync(StudyNote note)
        {
            _db.StudyNotes.Add(note);
            await _db.SaveChangesAsync();
            return note;
        }
    }
}
