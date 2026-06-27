using QnA.API.Models;

namespace QnA.API.Repositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Question>> GetByTagAsync(string tagName);
        Task<IEnumerable<Question>> GetRecentQuestionsAsync(int count);
    }
}