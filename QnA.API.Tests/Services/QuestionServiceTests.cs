using Moq;
using QnA.API.Models;
using QnA.API.Repositories.Interfaces;
using QnA.API.Services;
using Xunit;

namespace QnA.API.Tests.Services
{
    public class QuestionServiceTests
    {
        [Fact]
        public async Task GetQuestionByIdAsync_ReturnsQuestion_WhenQuestionExists()
        {
            // Arrange
            var mockRepository = new Mock<IQuestionRepository>();
            var expectedQuestion = new Question
            {
                Id = 1,
                Title = "Test Question",
                Content = "This is a test question",
                UserId = 1
            };
            
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedQuestion);
            
            var service = new QuestionService(mockRepository.Object);

            // Act
            var result = await service.GetQuestionByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Question", result.Title);
            Assert.Equal("This is a test question", result.Content);
            mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetRecentQuestionsAsync_ReturnsQuestions()
        {
            // Arrange
            var mockRepository = new Mock<IQuestionRepository>();
            var questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Title = "Question 1",
                    Content = "Content 1",
                    UserId = 1
                },
                new Question
                {
                    Id = 2,
                    Title = "Question 2",
                    Content = "Content 2",
                    UserId = 2
                }
            };
            
            mockRepository.Setup(repo => repo.GetRecentQuestionsAsync(2))
                .ReturnsAsync(questions);
            
            var service = new QuestionService(mockRepository.Object);

            // Act
            var result = await service.GetRecentQuestionsAsync(2);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("Question 1", resultList[0].Title);
            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Question 2", resultList[1].Title);
            mockRepository.Verify(repo => repo.GetRecentQuestionsAsync(2), Times.Once);
        }

        [Fact]
        public async Task CreateQuestionAsync_ReturnsCreatedQuestion()
        {
            // Arrange
            var mockRepository = new Mock<IQuestionRepository>();
            var question = new Question
            {
                Title = "New Question",
                Content = "New Content",
                UserId = 1
            };
            
            var createdQuestion = new Question
            {
                Id = 1,
                Title = "New Question",
                Content = "New Content",
                UserId = 1,
                CreatedAt = DateTime.UtcNow
            };
            
            mockRepository.Setup(repo => repo.AddAsync(question))
                .ReturnsAsync(createdQuestion);
            
            var service = new QuestionService(mockRepository.Object);

            // Act
            var result = await service.CreateQuestionAsync(question);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Question", result.Title);
            Assert.Equal("New Content", result.Content);
            Assert.Equal(1, result.UserId);
            mockRepository.Verify(repo => repo.AddAsync(question), Times.Once);
        }
    }
}