using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API.Controllers;
using Application;
using Application.Services;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BookController(
                _mockBookService.Object,
                Mock.Of<IValidator<CreateBookRequest>>());
        }

        [Fact]
        public async Task GetById_ExistingBook_ReturnsOkResult()
        {
            // Arrange
            var bookId = 1;
            var expectedBook = new BookResponse(bookId, "Test Book", "Fiction", 1);
            _mockBookService.Setup(s => s.GetBookById(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _controller.GetById(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedBook, okResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingBook_ReturnsNotFound()
        {
            // Arrange
            var bookId = 1;
            _mockBookService.Setup(s => s.GetBookById(bookId))
                .ThrowsAsync(new InvalidOperationException("Book not found"));

            // Act
            var result = await _controller.GetById(bookId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<BookResponse>
            {
                new BookResponse(1, "Book 1", "Fiction", 1),
                new BookResponse(2, "Book 2", "Non-Fiction", 2)
            };
            _mockBookService.Setup(s => s.GetAllBooks())
                .ReturnsAsync(books);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<BookResponse>>(okResult.Value);
            Assert.Equal(2, returnedBooks.Count());
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var request = new CreateBookRequest(1, "New Book", "Fiction",  1);
            var createdBook = new BookResponse(1, request.Name, request.Style, request.AuthorId);
            _mockBookService.Setup(s => s.CreateBook(request))
                .ReturnsAsync(createdBook);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(BookController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(createdBook.Id, ((BookResponse)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var bookId = 1;
            var request = new UpdateBookRequest(bookId, "Updated Book", "Non-Fiction", 2);
            _mockBookService.Setup(s => s.UpdateBook(request))
                .ReturnsAsync(new BookResponse(bookId, request.Name, request.Style, request.AuthorId));

            // Act
            var result = await _controller.Update(bookId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var urlId = 1;
            var request = new UpdateBookRequest(2, "Updated Book", "Non-Fiction", 2);

            // Act
            var result = await _controller.Update(urlId, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID in URL does not match ID in body", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_NonExistingBook_ReturnsNotFound()
        {
            // Arrange
            var bookId = 1;
            var request = new UpdateBookRequest(
                Id: bookId,
                Name: "Non-existent Book",
                Style: "Fiction",
                AuthorId: 1);

            _mockBookService.Setup(s => s.UpdateBook(request))
                .ReturnsAsync((BookResponse)null);

            // Act
            var result = await _controller.Update(bookId, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingBook_ReturnsNoContent()
        {
            // Arrange
            var bookId = 1;
            _mockBookService.Setup(s => s.DeleteBook(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistingBook_ReturnsNotFound()
        {
            // Arrange
            var bookId = 1;
            _mockBookService.Setup(s => s.DeleteBook(bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}