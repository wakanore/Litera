using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure;
using Moq;
using Application.Exceptions;
using Xunit;

namespace Application.Tests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly Mock<IValidator<CreateAuthorRequest>> _mockCreateValidator;
        private readonly Mock<IValidator<UpdateAuthorRequest>> _mockUpdateValidator;
        private readonly AuthorService _service;

        public AuthorServiceTests()
        {
            _mockRepo = new Mock<IAuthorRepository>();
            _mockCreateValidator = new Mock<IValidator<CreateAuthorRequest>>();
            _mockUpdateValidator = new Mock<IValidator<UpdateAuthorRequest>>();

            _service = new AuthorService(
                _mockRepo.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object);
        }

        private Author CreateTestAuthor(int id = 1)
        {
            return new Author { Id = id, Name = "Test Author", Phone = "1234567890" };
        }

        private AuthorResponse CreateTestAuthorResponse(int id = 1)
        {
            return new AuthorResponse(id, "Test Author", "1234567890");
        }

        [Fact]
        public async Task CreateAuthor_ValidRequest_ReturnsAuthorResponse()
        {
            // Arrange
            var request = new CreateAuthorRequest(Id: 0, Name: "New Author", Phone: "1234567890");
            var author = new Author { Id = 1, Name = request.Name, Phone = request.Phone };

            _mockCreateValidator.Setup(v => v.ValidateAsync(request, default))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>()));

            _mockRepo.Setup(r => r.Add(It.IsAny<Author>()))
                .ReturnsAsync(author);

            // Act
            var result = await _service.CreateAuthor(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.Name, result.Name);
            Assert.Equal(author.Phone, result.Phone);

            _mockRepo.Verify(r => r.Add(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task CreateAuthor_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateAuthorRequest(Id: 0, Name: "", Phone: "invalid");
            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Phone", "Invalid phone format")
            };
            

            var failures = new List<FluentValidation.Results.ValidationFailure>
    {
        new FluentValidation.Results.ValidationFailure("Name", "Name is required"),
        new FluentValidation.Results.ValidationFailure("Phone", "Invalid phone format")
    };
            _mockCreateValidator.Setup(v => v.ValidateAsync(request, default))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult(failures));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
        async () => await _service.CreateAuthor(request));

            Assert.Equal(2, exception.Errors.Count());
        }

        [Fact]
        public async Task UpdateAuthor_ValidRequest_UpdatesAuthor()
        {
            // Arrange
            var authorId = 1;
            var request = new UpdateAuthorRequest(Id: authorId, Name : "Updated Name", Phone : "9876543210"); 
            var existingAuthor = CreateTestAuthor(authorId);

            _mockUpdateValidator.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>()));

            _mockRepo.Setup(r => r.GetById(authorId))
                .ReturnsAsync(existingAuthor);

            _mockRepo.Setup(r => r.Update(It.IsAny<Author>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAuthor(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Phone, result.Phone);

            _mockRepo.Verify(r => r.GetById(authorId), Times.Once);
            _mockRepo.Verify(r => r.Update(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthor_AuthorNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = 999;
            var request = new UpdateAuthorRequest ( Id : authorId, Name : "Updated Name", Phone : "9876543210" );

            _mockUpdateValidator.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>()));

            _mockRepo.Setup(r => r.GetById(authorId))
                .ReturnsAsync((Author)null);
                
            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAuthor(request));

            Assert.Equal($"Author with id {authorId} not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAuthor_ExistingAuthor_ReturnsTrue()
        {
            // Arrange
            var authorId = 1;
            var author = CreateTestAuthor(authorId);

            _mockRepo.Setup(r => r.GetById(authorId))
                .ReturnsAsync(author);

            _mockRepo.Setup(r => r.Delete(authorId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAuthor(authorId);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(authorId), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthor_AuthorNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = 999;

            _mockRepo.Setup(r => r.GetById(authorId))
                .ReturnsAsync((Author)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAuthor(authorId));

            Assert.Equal($"Author with ID {authorId} not found.", exception.Message);
            _mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAuthorById_ExistingAuthor_ReturnsAuthorResponse()
        {
            // Arrange
            var authorId = 1;
            var author = CreateTestAuthor(authorId);
            var expectedResponse = CreateTestAuthorResponse(authorId);

            _mockRepo.Setup(r => r.GetById(authorId))
                .ReturnsAsync(author);

            // Act
            var result = await _service.GetAuthorById(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            Assert.Equal(expectedResponse.Name, result.Name);
            Assert.Equal(expectedResponse.Phone, result.Phone);
        }

        [Fact]
        public async Task GetAuthorById_AuthorNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = 999;

            _mockRepo.Setup(r => r.GetById(authorId))
                .ReturnsAsync((Author)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetAuthorById(authorId));

            Assert.Equal($"Author with ID {authorId} not found.", exception.Message);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                CreateTestAuthor(1),
                CreateTestAuthor(2)
            };

            var expectedResponses = authors.Select(a => new AuthorResponse(a.Id, a.Name, a.Phone)).ToList();

            _mockRepo.Setup(r => r.GetAll())
                .ReturnsAsync(authors);

            // Act
            var result = await _service.GetAllAuthors();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var resultList = result.ToList();
            for (int i = 0; i < expectedResponses.Count; i++)
            {
                Assert.Equal(expectedResponses[i].Id, resultList[i].Id);
                Assert.Equal(expectedResponses[i].Name, resultList[i].Name);
                Assert.Equal(expectedResponses[i].Phone, resultList[i].Phone);
            }
        }

        [Fact]
        public async Task GetAllAuthors_NoAuthors_ReturnsEmptyList()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Author>());

            // Act
            var result = await _service.GetAllAuthors();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}