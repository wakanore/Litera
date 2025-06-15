using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Application;
using Application.Services;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorService> _mockService;
        private readonly AuthorController _controller;

        public AuthorControllerTests()
        {
            _mockService = new Mock<IAuthorService>();
            _controller = new AuthorController(_mockService.Object, Mock.Of<IValidator<CreateAuthorRequest>>());
        }

        [Fact]
        public void GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            var author = new AuthorResponse(id, "Test Author", "12345678910");
            _mockService
                .Setup(s => s.GetAuthorById(It.Is<int>(id => id == 0)))
                .ReturnsAsync((AuthorResponse)null);

            // Act
            var result = _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(author, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<AuthorResponse>
            {
                new AuthorResponse(1, "Author 1", "12345678910"),
                new AuthorResponse(2, "Author 2", "12345678911")
            };
            _mockService.Setup(s => s.GetAllAuthors()).ReturnsAsync(authors);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAuthors = Assert.IsAssignableFrom<IEnumerable<AuthorResponse>>(okResult.Value);
            Assert.Equal(2, returnedAuthors.Count());
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var request = new CreateAuthorRequest ( 1, "New Author", "12345678910" );
            var response = new AuthorResponse(1, request.Name, request.Phone);
            _mockService.Setup(s => s.CreateAuthor(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(AuthorController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(response.Id, ((AuthorResponse)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var request = new UpdateAuthorRequest(1, "Updated Author", "12345678910");
            var expectedResponse = new AuthorResponse(request.Id, request.Name, request.Phone);

            _mockService
                .Setup(s => s.UpdateAuthor(request))
                .ReturnsAsync(expectedResponse); // Возвращаем AuthorResponse

            // Act
            var result = await _controller.Update(request.Id, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingAuthor_ReturnsNoContent()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(s => s.DeleteAuthor(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistingAuthor_ReturnsNotFound()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(s => s.DeleteAuthor(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Author with id {id} not found", notFoundResult.Value);
        }
    }
}