using System.Collections.Generic;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;

namespace AaramEducation.Core.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz?> GetWithQuestionsAsync(int quizId);
        Task<int> GetAttemptCountAsync(int studentId, int quizId);
        Task<IEnumerable<QuizAttempt>> GetAttemptsByStudentAsync(int studentId, int quizId);
        Task<QuizAttempt?> GetAttemptWithResponsesAsync(int attemptId);
        Task<QuizAttempt> StartAttemptAsync(int studentId, int quizId);
        Task<QuizAttempt> SubmitAttemptAsync(int attemptId, IEnumerable<(int questionId, int? optionId, string? text)> responses);
        Task<Quiz> CreateAsync(Quiz quiz);
        Task UpdateAsync(Quiz quiz);
        Task DeleteAsync(int quizId);
    }
}
