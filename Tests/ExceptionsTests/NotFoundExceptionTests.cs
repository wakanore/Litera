using Application.Exceptions;
using Xunit;

namespace UnitTests.Application.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void Constructor_WithMessageOnly_SetsMessageCorrectly()
        {
            // Arrange
            const string errorMessage = "Resource not found";

            // Act
            var exception = new NotFoundException(errorMessage);

            // Assert
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public void Constructor_WithNameAndKey_SetsFormattedMessage()
        {
            // Arrange
            const string entityName = "User";
            const int entityId = 123;

            // Act
            var exception = new NotFoundException(entityName, entityId);

            // Assert
            Assert.Equal($"Entity \"{entityName}\" ({entityId}) was not found.", exception.Message);
        }

        [Theory]
        [InlineData("Book", 1)]
        [InlineData("Author", "guid-123")]
        [InlineData("Order", 5.5)]
        public void Constructor_WithDifferentTypesOfKeys_GeneratesCorrectMessage(string name, object key)
        {
            // Act
            var exception = new NotFoundException(name, key);

            // Assert
            Assert.Equal($"Entity \"{name}\" ({key}) was not found.", exception.Message);
        }

        [Fact]
        public void Exception_IsSubclassOfException()
        {
            // Act
            var exception = new NotFoundException("Test");

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}