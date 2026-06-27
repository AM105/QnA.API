using QnA.API.Models;

namespace QnA.API.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionByIdAsync(int id);
        Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(int userId);
        Task<IEnumerable<Question>> GetQuestionsByTagAsync(string tagName);
        Task<IEnumerable<Question>> GetRecentQuestionsAsync(int count);
        Task<Question> CreateQuestionAsync(Question question);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);
    }
}