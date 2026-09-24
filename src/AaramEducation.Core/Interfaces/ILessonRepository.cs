using System.Collections.Generic;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;

namespace AaramEducation.Core.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetByIdAsync(int lessonId);
        Task<Lesson?> GetWithContentAsync(int lessonId);
        Task<IEnumerable<Lesson>> GetByModuleAsync(int moduleId);
        Task<Lesson> CreateAsync(Lesson lesson);
        Task UpdateAsync(Lesson lesson);
        Task DeleteAsync(int lessonId);
        Task<Video?> GetVideoAsync(int lessonId);
        Task<StudyNote?> GetStudyNoteAsync(int noteId);
        Task<Video> SaveVideoAsync(Video video);
        Task<StudyNote> SaveStudyNoteAsync(StudyNote note);
    }
}
