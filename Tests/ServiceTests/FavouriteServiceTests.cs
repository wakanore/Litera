using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Exceptions;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class FavouriteServiceTests
    {
        private readonly Mock<IFavouriteRepository> _mockRepository;
        private readonly Mock<IValidator<CreateFavouriteRequest>> _mockValidator;
        private readonly FavouriteService _service;

        public FavouriteServiceTests()
        {
            _mockRepository = new Mock<IFavouriteRepository>();
            _mockValidator = new Mock<IValidator<CreateFavouriteRequest>>();
            _service = new FavouriteService(_mockRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task AddFavourite_ValidRequest_ReturnsFavouriteResponse()
        {
            // Arrange
            var request = new CreateFavouriteRequest ( UserId : 1, BookId : 1 );
            var favourite = new Favourite { UserId = 1, BookId = 1 };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.Add(It.IsAny<Favourite>()))
                .ReturnsAsync(favourite);

            // Act
            var result = await _service.AddFavourite(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(favourite.UserId, result.UserId);
            Assert.Equal(favourite.BookId, result.BookId);
            _mockRepository.Verify(r => r.Add(It.IsAny<Favourite>()), Times.Once);
        }

        [Fact]
        public async Task AddFavourite_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateFavouriteRequest(UserId: 1, BookId: 1);
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("UserId", "User ID is required"),
                new ValidationFailure("BookId", "Book ID is required")
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Application.Exceptions.ValidationException>(
                () => _service.AddFavourite(request));

            Assert.Equal(2, exception.Errors.Count);
            _mockRepository.Verify(r => r.Add(It.IsAny<Favourite>()), Times.Never);
        }

        [Fact]
        public async Task FavouriteExists_ValidIds_ReturnsTrue()
        {
            // Arrange
            int userId = 1, bookId = 1;

            _mockRepository
                .Setup(r => r.FavouriteExists(userId, bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.FavouriteExists(userId, bookId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task FavouriteExists_ValidIds_ReturnsFalse()
        {
            // Arrange
            int userId = 1, bookId = 1;

            _mockRepository
                .Setup(r => r.FavouriteExists(userId, bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.FavouriteExists(userId, bookId);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(0, 1, "userId")]
        [InlineData(1, 0, "bookId")]
        [InlineData(-1, 1, "userId")]
        [InlineData(1, -1, "bookId")]
        public async Task FavouriteExists_InvalidIds_ThrowsArgumentException(
            int userId, int bookId, string paramName)
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.FavouriteExists(userId, bookId));

            Assert.Equal(paramName, exception.ParamName);
            _mockRepository.Verify(r => r.FavouriteExists(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteFavourite_ExistingFavourite_ReturnsTrue()
        {
            // Arrange
            int userId = 1, bookId = 1;

            _mockRepository
                .Setup(r => r.FavouriteExists(userId, bookId))
                .ReturnsAsync(true);

            _mockRepository
                .Setup(r => r.Delete(userId, bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteFavourite(userId, bookId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.Delete(userId, bookId), Times.Once);
        }

        [Fact]
        public async Task DeleteFavourite_NonExistingFavourite_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 1, bookId = 1;

            _mockRepository
                .Setup(r => r.FavouriteExists(userId, bookId))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteFavourite(userId, bookId));

            _mockRepository.Verify(r => r.Delete(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(0, 1, "userId")]
        [InlineData(1, 0, "bookId")]
        [InlineData(-1, 1, "userId")]
        [InlineData(1, -1, "bookId")]
        public async Task DeleteFavourite_InvalidIds_ThrowsArgumentException(
            int userId, int bookId, string paramName)
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.DeleteFavourite(userId, bookId));

            Assert.Equal(paramName, exception.ParamName);
            _mockRepository.Verify(r => r.FavouriteExists(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(r => r.Delete(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}