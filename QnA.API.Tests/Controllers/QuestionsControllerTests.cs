using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QnA.API.Controllers;
using QnA.API.Models;
using QnA.API.Services.Interfaces;
using System.Security.Claims;
using Xunit;

namespace QnA.API.Tests.Controllers
{
    public class QuestionsControllerTests
    {
        [Fact]
        public async Task GetQuestions_ReturnsOkResult_WithQuestions()
        {
            // Arrange
            var mockService = new Mock<IQuestionService>();
            var questions = new List<Question>
            {
                new Question { Id = 1, Title = "Question 1", Content = "Content 1" },
                new Question { Id = 2, Title = "Question 2", Content = "Content 2" }
            };
            
            mockService.Setup(service => service.GetAllQuestionsAsync())
                .ReturnsAsync(questions);

            var mockLogger = new Mock<ILogger<QuestionsController>>();
            var controller = new QuestionsController(mockService.Object, mockLogger.Object);
            // Act
            var result = await controller.GetQuestions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Question>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetQuestion_ReturnsOkResult_WhenQuestionExists()
        {
            // Arrange
            var mockService = new Mock<IQuestionService>();
            var question = new Question
            {
                Id = 1,
                Title = "Test Question",
                Content = "This is a test question"
            };
            
            mockService.Setup(service => service.GetQuestionByIdAsync(1))
                .ReturnsAsync(question);

            var mockLogger = new Mock<ILogger<QuestionsController>>();
            var controller = new QuestionsController(mockService.Object, mockLogger.Object);
            // Act
            var result = await controller.GetQuestion(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Question>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Question", returnValue.Title);
        }

        [Fact]
        public async Task GetQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IQuestionService>();
            mockService.Setup(service => service.GetQuestionByIdAsync(1))
                .ReturnsAsync((Question)null);

            var mockLogger = new Mock<ILogger<QuestionsController>>();
            var controller = new QuestionsController(mockService.Object, mockLogger.Object);
            // Act
            var result = await controller.GetQuestion(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetRecentQuestions_ReturnsOkResult_WithQuestions()
        {
            // Arrange
            var mockService = new Mock<IQuestionService>();
            var questions = new List<Question>
            {
                new Question { Id = 1, Title = "Question 1", Content = "Content 1" },
                new Question { Id = 2, Title = "Question 2", Content = "Content 2" }
            };
            
            mockService.Setup(service => service.GetRecentQuestionsAsync(2))
                .ReturnsAsync(questions);

            var mockLogger = new Mock<ILogger<QuestionsController>>();
            var controller = new QuestionsController(mockService.Object, mockLogger.Object);
            // Act
            var result = await controller.GetRecentQuestions(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Question>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }
    }
}