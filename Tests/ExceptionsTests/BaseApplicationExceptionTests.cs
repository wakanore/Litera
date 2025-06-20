using Application.Exceptions;
using Xunit;

namespace UnitTests.Application.Exceptions
{
    public class BaseApplicationExceptionTests
    {
        [Fact]
        public void Constructor_WithDefaultParameters_SetsCorrectValues()
        {
            // Arrange
            const string message = "Test error message";

            // Act
            var exception = new BaseApplicationException(message);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(400, exception.StatusCode);
            Assert.Equal("Bad Request", exception.Title);
        }

        [Fact]
        public void Constructor_WithCustomStatusCode_SetsCorrectStatusCode()
        {
            // Arrange
            const string message = "Test error message";
            const int statusCode = 404;

            // Act
            var exception = new BaseApplicationException(message, statusCode);

            // Assert
            Assert.Equal(statusCode, exception.StatusCode);
        }

        [Fact]
        public void Constructor_WithCustomTitle_SetsCorrectTitle()
        {
            // Arrange
            const string message = "Test error message";
            const string title = "Not Found";

            // Act
            var exception = new BaseApplicationException(message, title: title);

            // Assert
            Assert.Equal(title, exception.Title);
        }

        [Fact]
        public void Constructor_WithAllCustomParameters_SetsAllValuesCorrectly()
        {
            // Arrange
            const string message = "Custom error message";
            const int statusCode = 500;
            const string title = "Internal Server Error";

            // Act
            var exception = new BaseApplicationException(message, statusCode, title);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(title, exception.Title);
        }

        [Theory]
        [InlineData(400)]
        [InlineData(401)]
        [InlineData(404)]
        [InlineData(500)]
        public void Constructor_WithDifferentStatusCodes_SetsCorrectStatusCode(int statusCode)
        {
            // Arrange
            const string message = "Test message";

            // Act
            var exception = new BaseApplicationException(message, statusCode);

            // Assert
            Assert.Equal(statusCode, exception.StatusCode);
        }

        [Theory]
        [InlineData("Validation Error")]
        [InlineData("Authentication Failed")]
        [InlineData("Access Denied")]
        public void Constructor_WithDifferentTitles_SetsCorrectTitle(string title)
        {
            // Arrange
            const string message = "Test message";

            // Act
            var exception = new BaseApplicationException(message, title: title);

            // Assert
            Assert.Equal(title, exception.Title);
        }
    }
}