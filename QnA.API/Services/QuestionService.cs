using QnA.API.Models;
using QnA.API.Repositories.Interfaces;
using QnA.API.Services.Interfaces;

namespace QnA.API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(int userId)
        {
            return await _questionRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByTagAsync(string tagName)
        {
            return await _questionRepository.GetByTagAsync(tagName);
        }

        public async Task<IEnumerable<Question>> GetRecentQuestionsAsync(int count)
        {
            return await _questionRepository.GetRecentQuestionsAsync(count);
        }

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            return await _questionRepository.AddAsync(question);
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            await _questionRepository.UpdateAsync(question);
        }

        public async Task DeleteQuestionAsync(int id)
        {
            await _questionRepository.DeleteAsync(id);
        }
    }
}