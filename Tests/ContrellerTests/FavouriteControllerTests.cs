using System.Threading.Tasks;
using API.Controllers;
using Application;
using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentValidation.Results;

namespace API.Tests.Controllers
{
    public class FavouriteControllerTests
    {
        private readonly Mock<IFavouriteService> _mockFavouriteService;
        private readonly FavouriteController _controller;

        public FavouriteControllerTests()
        {
            _mockFavouriteService = new Mock<IFavouriteService>();
            _controller = new FavouriteController(
                _mockFavouriteService.Object,
                Mock.Of<IValidator<CreateFavouriteRequest>>());
        }

        [Fact]
        public async Task Delete_ExistingFavourite_ReturnsNoContent()
        {
            // Arrange
            int userId = 1;
            int bookId = 1;
            _mockFavouriteService.Setup(s => s.DeleteFavourite(userId, bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(userId, bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockFavouriteService.Verify(s => s.DeleteFavourite(userId, bookId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingFavourite_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;
            int bookId = 1;
            _mockFavouriteService.Setup(s => s.DeleteFavourite(userId, bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(userId, bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsTrue()
        {
            // Arrange
            var request = new CreateFavouriteRequest ( UserId : 1, BookId : 1 );
            _mockFavouriteService.Setup(s => s.AddFavourite(request))
                .ReturnsAsync(new FavouriteResponse(request.UserId, request.BookId));

            // Act
            var result = await _controller.Add(request);

            // Assert
            Assert.True(result);
            _mockFavouriteService.Verify(s => s.AddFavourite(request), Times.Once);
        }

        [Fact]
        public async Task Add_InvalidRequest_ReturnsFalse()
        {
            // Arrange
            var request = new CreateFavouriteRequest(1, 1);
            var validationFailures = new[]
            {
                new ValidationFailure("UserId", "User ID is required"),
                new ValidationFailure("BookId", "Book ID is required")
            };

            var validator = new Mock<IValidator<CreateFavouriteRequest>>();
            validator.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var controller = new FavouriteController(
                _mockFavouriteService.Object,
                validator.Object);

            // Act
            var result = await controller.Add(request);

            // Assert
            Assert.False(result);
            _mockFavouriteService.Verify(
                s => s.AddFavourite(It.IsAny<CreateFavouriteRequest>()),
                Times.Never);
        }
    }
}