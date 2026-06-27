using QnA.API.Models;
using QnA.API.Repositories;
using QnA.API.Tests.Helpers;
using Xunit;

namespace QnA.API.Tests.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetByUsernameAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var context = DbContextHelper.CreateInMemoryDbContext();
            var repository = new UserRepository(context);
            
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            
            await repository.AddAsync(user);

            // Act
            var result = await repository.GetByUsernameAsync("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task GetByUsernameAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var context = DbContextHelper.CreateInMemoryDbContext();
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetByUsernameAsync("nonexistentuser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var context = DbContextHelper.CreateInMemoryDbContext();
            var repository = new UserRepository(context);
            
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            
            await repository.AddAsync(user);

            // Act
            var result = await repository.GetByEmailAsync("test@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
        }
    }
}