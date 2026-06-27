using Moq;
using QnA.API.Models;
using QnA.API.Repositories.Interfaces;
using QnA.API.Services;
using Xunit;

namespace QnA.API.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var expectedUser = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com"
            };
            
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedUser);
            
            var service = new UserService(mockRepository.Object);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
            mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((User)null);
            
            var service = new UserService(mockRepository.Object);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.Null(result);
            mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsCreatedUser()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var user = new User
            {
                Username = "newuser",
                Email = "new@example.com"
            };
            
            var createdUser = new User
            {
                Id = 1,
                Username = "newuser",
                Email = "new@example.com"
            };
            
            mockRepository.Setup(repo => repo.AddAsync(user))
                .ReturnsAsync(createdUser);
            
            var service = new UserService(mockRepository.Object);

            // Act
            var result = await service.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("newuser", result.Username);
            Assert.Equal("new@example.com", result.Email);
            mockRepository.Verify(repo => repo.AddAsync(user), Times.Once);
        }
    }
}