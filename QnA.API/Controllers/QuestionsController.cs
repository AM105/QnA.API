using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QnA.API.Models;
using QnA.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace QnA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(IQuestionService questionService, ILogger<QuestionsController> logger)
        {
            _questionService = questionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            _logger.LogInformation("Getting all questions");
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            _logger.LogInformation("Getting question with ID: {QuestionId}", id);
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                _logger.LogWarning("Question with ID: {QuestionId} not found", id);
                return NotFound();
            }
            return Ok(question);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionsByUser(int userId)
        {
            _logger.LogInformation("Getting questions for user with ID: {UserId}", userId);
            var questions = await _questionService.GetQuestionsByUserIdAsync(userId);
            return Ok(questions);
        }

        [HttpGet("tag/{tagName}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionsByTag(string tagName)
        {
            _logger.LogInformation("Getting questions with tag: {TagName}", tagName);
            var questions = await _questionService.GetQuestionsByTagAsync(tagName);
            return Ok(questions);
        }

        [HttpGet("recent/{count}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetRecentQuestions(int count)
        {
            _logger.LogInformation("Getting {Count} recent questions", count);
            var questions = await _questionService.GetRecentQuestionsAsync(count);
            return Ok(questions);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion(Question question)
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            _logger.LogInformation("User {UserId} creating a new question: {QuestionTitle}", userId, question.Title);
            
            // Set the user ID from the authenticated user
            question.UserId = userId;
            
            var createdQuestion = await _questionService.CreateQuestionAsync(question);
            _logger.LogInformation("Question created with ID: {QuestionId}", createdQuestion.Id);
            
            return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, Question question)
        {
            if (id != question.Id)
            {
                _logger.LogWarning("Bad request: ID mismatch. Path ID: {PathId}, Question ID: {QuestionId}", id, question.Id);
                return BadRequest();
            }

            // Check if the user is updating their own question
            var existingQuestion = await _questionService.GetQuestionByIdAsync(id);
            if (existingQuestion == null)
            {
                _logger.LogWarning("Question with ID: {QuestionId} not found", id);
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            if (existingQuestion.UserId != userId)
            {
                _logger.LogWarning("User {UserId} attempted to update question {QuestionId} owned by user {OwnerUserId}", 
                    userId, id, existingQuestion.UserId);
                return Forbid();
            }

            _logger.LogInformation("User {UserId} updating question with ID: {QuestionId}", userId, id);
            await _questionService.UpdateQuestionAsync(question);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            // Check if the user is deleting their own question
            var existingQuestion = await _questionService.GetQuestionByIdAsync(id);
            if (existingQuestion == null)
            {
                _logger.LogWarning("Question with ID: {QuestionId} not found", id);
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            if (existingQuestion.UserId != userId)
            {
                _logger.LogWarning("User {UserId} attempted to delete question {QuestionId} owned by user {OwnerUserId}", 
                    userId, id, existingQuestion.UserId);
                return Forbid();
            }

            _logger.LogInformation("User {UserId} deleting question with ID: {QuestionId}", userId, id);
            await _questionService.DeleteQuestionAsync(id);
            return NoContent();
        }
    }
}



