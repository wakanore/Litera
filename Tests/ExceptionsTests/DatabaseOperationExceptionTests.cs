using Application.Exceptions;
using System;
using Xunit;

namespace UnitTests.Application.Exceptions
{
    public class DatabaseOperationExceptionTests
    {
        [Fact]
        public void Constructor_WithMessageOnly_SetsMessageCorrectly()
        {
            // Arrange
            const string errorMessage = "Database operation failed";

            // Act
            var exception = new DatabaseOperationException(errorMessage);

            // Assert
            Assert.Equal(errorMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsBothCorrectly()
        {
            // Arrange
            const string errorMessage = "Database operation failed";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new DatabaseOperationException(errorMessage, innerException);

            // Assert
            Assert.Equal(errorMessage, exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Theory]
        [InlineData("Connection timeout")]
        [InlineData("Constraint violation")]
        [InlineData("Deadlock occurred")]
        public void Constructor_WithDifferentMessages_SetsMessageCorrectly(string message)
        {
            // Act
            var exception = new DatabaseOperationException(message);

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void Constructor_WithNullMessage_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DatabaseOperationException(null));
        }

        [Fact]
        public void Constructor_WithNullMessageAndValidInnerException_ThrowsArgumentNullException()
        {
            // Arrange
            var innerException = new Exception("Test inner");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DatabaseOperationException(null, innerException));
        }

        [Fact]
        public void Constructor_WithValidMessageAndNullInnerException_SetsMessageAndNullInner()
        {
            // Arrange
            const string message = "Valid message";

            // Act
            var exception = new DatabaseOperationException(message, null);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Exception_IsSubclassOfException()
        {
            // Arrange & Act
            var exception = new DatabaseOperationException("Test");

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

    }
}