using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Services;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure;
using Moq;
using Xunit;

namespace Application.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockRepository;
        private readonly Mock<IValidator<CreateBookRequest>> _mockCreateValidator;
        private readonly Mock<IValidator<UpdateBookRequest>> _mockUpdateValidator;
        private readonly BookService _service;

        public BookServiceTests()
        {
            _mockRepository = new Mock<IBookRepository>();
            _mockCreateValidator = new Mock<IValidator<CreateBookRequest>>();
            _mockUpdateValidator = new Mock<IValidator<UpdateBookRequest>>();
            _service = new BookService(
                _mockRepository.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object);
        }

        [Fact]
        public async Task CreateBook_ValidRequest_ReturnsBookResponse()
        {
            // Arrange
            var request = new CreateBookRequest (Id:1, Name : "Test Book", Style : "Fiction", AuthorId : 1 );
            var book = new Book { Id = 1, Name = request.Name, Style = request.Style, AuthorId = request.AuthorId };

            _mockCreateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.Add(It.IsAny<Book>()))
                .ReturnsAsync(book);

            // Act
            var result = await _service.CreateBook(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Name, result.Name);
            Assert.Equal(book.Style, result.Style);
            Assert.Equal(book.AuthorId, result.AuthorId);
            _mockRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task CreateBook_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            CreateBookRequest request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateBook(request));
        }

        [Fact]
        public async Task CreateBook_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateBookRequest(Id: 1, Name: "Test Book", Style: "Fiction", AuthorId: 1);
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            };

            _mockCreateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Application.Exceptions.ValidationException>(
        () => _service.CreateBook(request));

            Assert.Single(exception.Errors);
            _mockRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task UpdateBook_ValidRequest_ReturnsUpdatedBookResponse()
        {
            // Arrange
            var request = new UpdateBookRequest ( Id : 1, Name : "Updated Book", Style : "Non-Fiction", AuthorId : 2 );
            var existingBook = new Book { Id = 1, Name = "Original Book", Style = "Fiction", AuthorId = 1 };

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.GetById(request.Id))
                .ReturnsAsync(existingBook);

            _mockRepository
                .Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(true);

            // Or for failure case
            _mockRepository
                .Setup(r => r.Update(It.IsAny<Book>()))
                .ReturnsAsync(false);

            // Act
            var result = await _service.UpdateBook(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Style, result.Style);
            Assert.Equal(request.AuthorId, result.AuthorId);
            _mockRepository.Verify(r => r.Update(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBook_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new UpdateBookRequest ( Id : 1 , Name:"", AuthorId: 1, Style:"");

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository
                .Setup(r => r.GetById(request.Id))
                .ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateBook(request));
            _mockRepository.Verify(r => r.Update(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBook_ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            var book = new Book { Id = id };

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync(book);

            _mockRepository
                .Setup(r => r.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteBook(id);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public async Task DeleteBook_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            int id = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteBook(id));
            _mockRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBook_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int id = 1;

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBook(id));
            _mockRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetBookById_ValidId_ReturnsBookResponse()
        {
            // Arrange
            int id = 1;
            var book = new Book { Id = id, Name = "Test Book", Style = "Fiction", AuthorId = 1 };

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync(book);

            // Act
            var result = await _service.GetBookById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Name, result.Name);
            Assert.Equal(book.Style, result.Style);
            Assert.Equal(book.AuthorId, result.AuthorId);
        }

        [Fact]
        public async Task GetBookById_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            int id = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetBookById(id));
        }

        [Fact]
        public async Task GetBookById_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int id = 1;

            _mockRepository
                .Setup(r => r.GetById(id))
                .ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetBookById(id));
        }

        [Fact]
        public async Task GetAllBooks_BooksExist_ReturnsBookResponses()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Book 1", Style = "Fiction", AuthorId = 1 },
                new Book { Id = 2, Name = "Book 2", Style = "Non-Fiction", AuthorId = 2 }
            };

            _mockRepository
                .Setup(r => r.GetAll())
                .ReturnsAsync(books);

            // Act
            var result = await _service.GetAllBooks();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAllBooks_NoBooks_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<Book>();

            _mockRepository
                .Setup(r => r.GetAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _service.GetAllBooks();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}