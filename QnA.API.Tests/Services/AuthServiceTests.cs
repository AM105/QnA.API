using Moq;
using QnA.API.DTOs;
using QnA.API.Helpers;
using QnA.API.Models;
using QnA.API.Repositories.Interfaces;
using QnA.API.Services;
using QnA.API.Services.Interfaces;
using Xunit;

namespace QnA.API.Tests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ReturnsSuccess_WhenUserDoesNotExist()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockJwtHelper = new Mock<JwtHelper>(null);
            
            mockUserRepository.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            
            mockUserRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            
            mockPasswordService.Setup(service => service.HashPassword(It.IsAny<string>()))
                .Returns("hashedpassword");
            
            var createdUser = new User
            {
                Id = 1,
                Username = "newuser",
                Email = "new@example.com",
                PasswordHash = "hashedpassword"
            };
            
            mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(createdUser);
            
            mockJwtHelper.Setup(helper => helper.GenerateJwtToken(It.IsAny<User>()))
                .Returns("jwt-token");
            
            var service = new AuthService(
                mockUserRepository.Object,
                mockPasswordService.Object,
                mockJwtHelper.Object);
            
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "password123"
            };

            // Act
            var result = await service.RegisterAsync(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Registration successful", result.Message);
            Assert.Equal("jwt-token", result.Token);
            Assert.Equal(1, result.UserId);
            Assert.Equal("newuser", result.Username);
            Assert.Equal("new@example.com", result.Email);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailure_WhenUsernameExists()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockJwtHelper = new Mock<JwtHelper>(null);
            
            var existingUser = new User
            {
                Id = 1,
                Username = "existinguser",
                Email = "existing@example.com"
            };
            
            mockUserRepository.Setup(repo => repo.GetByUsernameAsync("existinguser"))
                .ReturnsAsync(existingUser);
            
            var service = new AuthService(
                mockUserRepository.Object,
                mockPasswordService.Object,
                mockJwtHelper.Object);
            
            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Email = "new@example.com",
                Password = "password123"
            };

            // Act
            var result = await service.RegisterAsync(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Username already exists", result.Message);
            Assert.Null(result.Token);
        }

        [Fact]
        public async Task LoginAsync_ReturnsSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordService = new Mock<IPasswordService>();
            var mockJwtHelper = new Mock<JwtHelper>(null);
            
            var existingUser = new User
            {
                Id = 1,
                Username = "existinguser",
                Email = "existing@example.com",
                PasswordHash = "hashedpassword"
            };
            
            mockUserRepository.Setup(repo => repo.GetByUsernameAsync("existinguser"))
                .ReturnsAsync(existingUser);
            
            mockPasswordService.Setup(service => service.VerifyPassword("password123", "hashedpassword"))
                .Returns(true);
            
            mockJwtHelper.Setup(helper => helper.GenerateJwtToken(It.IsAny<User>()))
                .Returns("jwt-token");
            
            var service = new AuthService(
                mockUserRepository.Object,
                mockPasswordService.Object,
                mockJwtHelper.Object);
            
            var loginDto = new LoginDto
            {
                Username = "existinguser",
                Password = "password123"
            };

            // Act
            var result = await service.LoginAsync(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Login successful", result.Message);
            Assert.Equal("jwt-token", result.Token);
            Assert.Equal(1, result.UserId);
            Assert.Equal("existinguser", result.Username);
            Assert.Equal("existing@example.com", result.Email);
        }
    }
}