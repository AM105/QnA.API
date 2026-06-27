using Microsoft.EntityFrameworkCore;
using QnA.API.Data;
using QnA.API.Models;
using QnA.API.Repositories.Interfaces;

namespace QnA.API.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Question>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(q => q.UserId == userId)
                .Include(q => q.User)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetByTagAsync(string tagName)
        {
            return await _dbSet
                .Where(q => q.QuestionTags!.Any(qt => qt.Tag!.Name == tagName))
                .Include(q => q.User)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetRecentQuestionsAsync(int count)
        {
            return await _dbSet
                .Include(q => q.User)
                .OrderByDescending(q => q.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}