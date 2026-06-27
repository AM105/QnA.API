using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QnA.API.Controllers;
using QnA.API.DTOs;
using QnA.API.Services.Interfaces;
using Xunit;

namespace QnA.API.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ILogger<AuthController>> _mockLogger;

        public AuthControllerTests()
        {
            _mockLogger = new Mock<ILogger<AuthController>>();
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "password123"
            };
            
            var authResponse = new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Token = "jwt-token",
                UserId = 1,
                Username = "newuser",
                Email = "new@example.com"
            };
            
            mockAuthService.Setup(service => service.RegisterAsync(registerDto))
                .ReturnsAsync(authResponse);
            
            var controller = new AuthController(mockAuthService.Object, _mockLogger.Object);

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal("Registration successful", returnValue.Message);
            Assert.Equal("jwt-token", returnValue.Token);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Email = "existing@example.com",
                Password = "password123"
            };
            
            var authResponse = new AuthResponseDto
            {
                Success = false,
                Message = "Username already exists"
            };
            
            mockAuthService.Setup(service => service.RegisterAsync(registerDto))
                .ReturnsAsync(authResponse);
            
            var controller = new AuthController(mockAuthService.Object, _mockLogger.Object);

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = Assert.IsType<AuthResponseDto>(badRequestResult.Value);
            Assert.False(returnValue.Success);
            Assert.Equal("Username already exists", returnValue.Message);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var loginDto = new LoginDto
            {
                Username = "existinguser",
                Password = "password123"
            };
            
            var authResponse = new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = "jwt-token",
                UserId = 1,
                Username = "existinguser",
                Email = "existing@example.com"
            };
            
            mockAuthService.Setup(service => service.LoginAsync(loginDto))
                .ReturnsAsync(authResponse);
            
            var controller = new AuthController(mockAuthService.Object, _mockLogger.Object);

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal("Login successful", returnValue.Message);
            Assert.Equal("jwt-token", returnValue.Token);
        }
    }
}
