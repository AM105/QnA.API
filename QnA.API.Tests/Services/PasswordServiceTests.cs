using QnA.API.Services;
using Xunit;

namespace QnA.API.Tests.Services
{
    public class PasswordServiceTests
    {
        [Fact]
        public void HashPassword_ReturnsDifferentHash_ForDifferentPasswords()
        {
            // Arrange
            var service = new PasswordService();
            var password1 = "password1";
            var password2 = "password2";

            // Act
            var hash1 = service.HashPassword(password1);
            var hash2 = service.HashPassword(password2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void HashPassword_ReturnsSameHash_ForSamePassword()
        {
            // Arrange
            var service = new PasswordService();
            var password = "password123";

            // Act
            var hash1 = service.HashPassword(password);
            var hash2 = service.HashPassword(password);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ReturnsTrue_WhenPasswordMatches()
        {
            // Arrange
            var service = new PasswordService();
            var password = "password123";
            var hash = service.HashPassword(password);

            // Act
            var result = service.VerifyPassword(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ReturnsFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var service = new PasswordService();
            var password = "password123";
            var wrongPassword = "wrongpassword";
            var hash = service.HashPassword(password);

            // Act
            var result = service.VerifyPassword(wrongPassword, hash);

            // Assert
            Assert.False(result);
        }
    }
}