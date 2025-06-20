using Application.Exceptions;
using FluentValidation.Results;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Application.Exceptions
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void Constructor_WithValidationFailures_SetsPropertiesCorrectly()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Age", "Age must be positive")
            };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.Equal("Ошибка валидации", exception.Message);
            Assert.Equal(400, exception.StatusCode);
            Assert.Equal("Validation Error", exception.Title);
            Assert.Equal(2, exception.Errors.Count);
            Assert.Contains("Name: Name is required", exception.Errors);
            Assert.Contains("Age: Age must be positive", exception.Errors);
        }

        [Fact]
        public void Constructor_WithEmptyFailuresList_SetsEmptyErrorsList()
        {
            // Arrange
            var failures = new List<ValidationFailure>();

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.Empty(exception.Errors);
        }

        [Fact]
        public void Constructor_WithNullFailures_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ValidationException(null));
        }

        [Fact]
        public void Exception_InheritsFromBaseApplicationException()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Test", "Test error")
            };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.IsAssignableFrom<BaseApplicationException>(exception);
        }

        [Theory]
        [InlineData("Email", "Invalid email format")]
        [InlineData("Password", "Password too weak")]
        [InlineData("", "General error")]
        public void Constructor_WithSingleFailure_FormatsErrorCorrectly(string property, string error)
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure(property, error)
            };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            var expectedError = string.IsNullOrEmpty(property)
                ? $"{error}"
                : $"{property}: {error}";
            Assert.Equal(expectedError, exception.Errors.Single());
        }

        [Fact]
        public void Errors_AreImmutable()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Test", "Test error")
            };
            var exception = new ValidationException(failures);

            // Act
            var errors = exception.Errors;
            errors.Add("New error");

            // Assert
            Assert.Single(exception.Errors); // Original list should remain unchanged
        }
    }
}